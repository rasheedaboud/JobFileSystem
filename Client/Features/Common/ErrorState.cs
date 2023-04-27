using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;

namespace Client.Features
{

    public record ClearErrorAction { }

    public record SetErrorAction
    {
        private static string _defaultMessage => "Unable to perform selected action. Check internet connection and try again.";
        private static string _devMessage(Exception ex) => ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace;

        public SetErrorAction(Exception ex=null,string displayMessage = null)
        {
            DeveloperMessage = ex is null ? _defaultMessage : _devMessage(ex);
            DisplayMessage = displayMessage is null ? _defaultMessage : displayMessage;
            Exception = ex;
        }

        public string DeveloperMessage { get; set; }
        public string DisplayMessage { get; }
        public Exception Exception { get; }
    }

    public record ErrorState
    {
        public ErrorState(string currentErrorMessage,string displayMessage=null)
        {
            DeveloperMessage = currentErrorMessage;
            if (currentErrorMessage != null)
            {
                DisplayMessage = displayMessage is null ? "Unable to perform selected action. Check internet connection and try again." : displayMessage;
            }
        }
        public string DeveloperMessage { get; init; }
        public string DisplayMessage { get; init; }

        public bool HasCurrentErrors => !string.IsNullOrWhiteSpace(DisplayMessage);
    }
    public static class Reducers
    {
        [ReducerMethod]
        public static ErrorState SetErrorState(ErrorState state, SetErrorAction action) => state with { DeveloperMessage = action.DeveloperMessage, DisplayMessage = action.DisplayMessage };
        

        [ReducerMethod]
        public static ErrorState ClearErrorState(ErrorState state, ClearErrorAction _) =>
            new ErrorState(null,null);
    }

    public class ErrorFeature : Feature<ErrorState>
    {
        public override string GetName() => nameof(ErrorState);

        protected override ErrorState GetInitialState() =>
            new ErrorState(currentErrorMessage: null,displayMessage:null);
    }


    public class SetErrorEffect : Effect<SetErrorAction>
    {
        private readonly ILogger<SetErrorEffect> _logger;

        public SetErrorEffect(ILogger<SetErrorEffect> logger)
        {
            _logger = logger;
        }

        public override async Task HandleAsync(SetErrorAction action, IDispatcher dispatcher)
        {
            try
            {
                _logger.LogError(action.Exception,default);
                dispatcher.Dispatch(new FailureAction());

            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new SetErrorAction(e,null));
            }

        }
    }
}
