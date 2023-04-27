using Ardalis.SmartEnum;

namespace JobFileSystem.Shared.Enums
{
    public sealed class MaterialForm : SmartEnum<MaterialForm,ushort>
    {
        public static readonly MaterialForm Plate = new (nameof(Plate), 1);
        public static readonly MaterialForm Pipe = new (nameof(Pipe), 2);
        public static readonly MaterialForm Valve = new (nameof(Valve), 3);
        public static readonly MaterialForm Fitting = new(nameof(Fitting), 4);
        public static readonly MaterialForm IBeam = new("I Beam", 5);
        public static readonly MaterialForm AngleIron = new("Angle Iron", 6);
        public static readonly MaterialForm CChannel = new("C Channel", 7);
        public static readonly MaterialForm SquareTubbing = new("Square Tubbng", 8);
        public static readonly MaterialForm Tube = new(nameof(Tube), 9);
        public static readonly MaterialForm FlatBar = new("Flat Bar", 10);

        private MaterialForm(string name, ushort value) : base(name, value)
        {
        }
    }
}
