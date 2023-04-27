using Ardalis.SmartEnum;

namespace JobFileSystem.Shared.Enums
{

    public abstract class JobStatus : SmartEnum<JobStatus>
    {
        public static readonly JobStatus New = new NewStatus();
        public static readonly JobStatus Review = new InitialReviewStatus();
        public static readonly JobStatus Accepted = new AcceptedStatus();
        public static readonly JobStatus Inprogress = new InProgressStatus();
        public static readonly JobStatus Complete = new CompleteStatus();
        public static readonly JobStatus Paid = new PaidStatus();
        public static readonly JobStatus Cancelled = new CancelledStatus();

        private const string _new = "New";
        private const string _submittedForInitialReview = "Under Review";
        private const string _accepted = "Accepted";
        private const string _inProgress = "In progress";
        private const string _complete = "Complete";
        private const string _paid = "Paid";
        private const string _cancelled = "Cancelled";

        private JobStatus(string name, int value) : base(name, value)
        {
        }
        public abstract bool CanTransitionTo(JobStatus next);

        private sealed class NewStatus : JobStatus
        {
            public NewStatus() : base(_new, 0)
            {
            }

            public override bool CanTransitionTo(JobStatus next) =>
                next == JobStatus.Review || next == JobStatus.Cancelled;
        }
        private sealed class InitialReviewStatus : JobStatus
        {
            public InitialReviewStatus() : base(_submittedForInitialReview, 1)
            {
            }

            public override bool CanTransitionTo(JobStatus next) =>
                next == JobStatus.Accepted || next == JobStatus.Cancelled;
        }
        private sealed class AcceptedStatus : JobStatus
        {
            public AcceptedStatus() : base(_accepted, 2)
            {
            }

            public override bool CanTransitionTo(JobStatus next) =>
                next == JobStatus.Inprogress || next == JobStatus.Cancelled;
        }
        private sealed class InProgressStatus : JobStatus
        {
            public InProgressStatus() : base(_inProgress, 3)
            {
            }

            public override bool CanTransitionTo(JobStatus next) =>
                next == JobStatus.Complete || next == JobStatus.Cancelled;
        }
        private sealed class CompleteStatus : JobStatus
        {
            public CompleteStatus() : base(_complete, 4)
            {
            }

            public override bool CanTransitionTo(JobStatus next) =>
                next == JobStatus.Complete || next == JobStatus.Cancelled;
        }

        private sealed class PaidStatus : JobStatus
        {
            public PaidStatus() : base(_paid, 5)
            {
            }

            public override bool CanTransitionTo(JobStatus next) =>
                next == JobStatus.Cancelled;
        }

        private sealed class CancelledStatus : JobStatus
        {
            public CancelledStatus() : base(_cancelled, 6)
            {
            }

            public override bool CanTransitionTo(JobStatus next) =>
                false;
        }
    }
}
