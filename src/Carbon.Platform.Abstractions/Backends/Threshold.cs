namespace Carbon.Platform
{
    public class Threshold
    {
        private int a;
        private int b;

        public Threshold(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public static Threshold Parse(string text)
        {
            var parts = text.Split('/');

            return new Threshold(
                a: int.Parse(parts[0]),
                b: int.Parse(parts[1])
            );
        }

        // e.g. 5/5

        public override string ToString()
        {
            return a + "/" + b;
        }
    }
}
