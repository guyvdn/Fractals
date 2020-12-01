namespace Fractals
{
    public sealed record Area
    {
        public double X1 { get; }
        public double X2 { get; }
        public double Y1 { get; }
        public double Y2 { get; }

        public Area(double x1, double x2, double y1, double y2) => (X1, X2, Y1, Y2) = (x1, x2, y1, y2);

        public double Width => X2 - X1;
        public double Height => Y2 - Y1;
    }
}