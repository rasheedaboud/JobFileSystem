using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFileSystem.Shared.Enums
{
    public abstract class EstimateStatus : SmartEnum<EstimateStatus>
    {
        public static readonly EstimateStatus New = new NewStatus();
        public static readonly EstimateStatus InitialReview = new InitialReviewStatus();
        public static readonly EstimateStatus Accepted = new AcceptedStatus();
        public static readonly EstimateStatus JobFileIssued = new JobFileIssuedStatus();
        public static readonly EstimateStatus Cancelled = new CancelledStatus();

        private const string _new = "New";
        private const string _submittedForInitialReview = "Under Review";
        private const string _accepted = "Accepted";
        private const string _iussued = "Job File Issued";
        private const string _cancelled = "Cancelled";

        private EstimateStatus(string name, int value) : base(name, value)
        {
        }
        public abstract bool CanTransitionTo(EstimateStatus next);



        private sealed class NewStatus : EstimateStatus
        {
            public NewStatus() : base(_new, 0)
            {
            }

            public override bool CanTransitionTo(EstimateStatus next) =>
                next == EstimateStatus.InitialReview || next == EstimateStatus.Cancelled;
        }
        private sealed class InitialReviewStatus : EstimateStatus
        {
            public InitialReviewStatus() : base(_submittedForInitialReview, 1)
            {
            }

            public override bool CanTransitionTo(EstimateStatus next) =>
                next == EstimateStatus.Accepted || next == EstimateStatus.Cancelled;
        }
        private sealed class AcceptedStatus : EstimateStatus
        {
            public AcceptedStatus() : base(_accepted, 2)
            {
            }

            public override bool CanTransitionTo(EstimateStatus next) =>
                next == EstimateStatus.Cancelled || next == EstimateStatus.JobFileIssued;
        }
        private sealed class JobFileIssuedStatus : EstimateStatus
        {
            public JobFileIssuedStatus() : base(_iussued, 3)
            {
            }

            public override bool CanTransitionTo(EstimateStatus next) =>
                next == EstimateStatus.Cancelled;
        }
        private sealed class CancelledStatus : EstimateStatus
        {
            public CancelledStatus() : base(_cancelled, 4)
            {
            }
            public override bool CanTransitionTo(EstimateStatus next) =>
                false;
        }
    }
}
