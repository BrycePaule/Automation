namespace bpdev
{
    [System.Serializable]
    public class NoiseFreqAmp
    {
        public float Frequency;
        public float Amplitude;

        public NoiseFreqAmp(float _freq, float _amp)
        {
            Frequency = _freq;
            Amplitude = _amp;
        }
    }
}