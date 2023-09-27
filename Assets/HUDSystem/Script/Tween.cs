using System;

namespace AnimationTween
{
    public static class Tween
    {
        static float Flip(float x)
        {
            return 1 - x;
        }

        static float Square(float x)
        {
            return x * x;
        }

        static float Lerp(float start_value, float end_value, float pct)
        {
            return start_value + (end_value - start_value) * pct;
        }

        static float EaseInSine(float x)
        {
            return 1 - (float)Math.Cos(x * Math.PI / 2);
        }

        static float EaseOutSine(float x)
        {
            return (float)Math.Sin(x * Math.PI / 2);
        }



        public static float EaseIn(float t, float e)
        {
            return (float)Math.Pow(t, e);
        }

        public static float EaseIn(float t)
        {
            return t * t;
        }



        public static float EaseOut(float t, float e)
        {
            return Flip((float)Math.Pow(Flip(t), e));
        }

        public static float EaseOut(float t)
        {
            return Flip((float)Square(Flip(t)));
        }



        public static float EaseInOut(float t, float in_speed, float out_speed)
        {
            return Lerp(EaseIn(t, in_speed), EaseOut(t, out_speed), t);
        }

        public static float EaseInOut(float t)
        {
            return Lerp(EaseIn(t), EaseOut(t), t);
        }

        public static float Spike(float t, float forward_speed, float backward_speed, float peak, float waiting)
        {
            if (t < peak)
                return EaseIn(t, forward_speed);

            if (t > peak + waiting)
                return EaseIn(Flip(t), backward_speed);
            else
                return t;
        }

        public static float Spike(float t, float forward_speed, float backward_speed, float peak)
        {
            if (t < peak)
                return EaseIn(t, forward_speed);
            return EaseIn(Flip(t), backward_speed);
        }

        public static float Spike(float t, float speed)
        {
            if (t < 0.5f)
                return EaseIn(t, speed);
            return EaseIn(Flip(t), speed);
        }

        public static float Spike(float t)
        {
            if (t < 0.5f)
                return EaseIn(t);
            return EaseIn(Flip(t));
        }



        public static float SineInOut(float t)
        {
            return Lerp(EaseInSine(t), EaseOutSine(t), t);
        }
    }
}

