namespace OWT.CryptoCom.Dto {
    public class SecondPass
    {
        public bool Passed { get; set; }
        public static implicit operator bool(SecondPass x) => x.Passed;
    }
}
