using System;
using System.Collections.Generic;
using System.Text;

namespace CustomizableAnalysisLibrary
{
    public static class Peaks
    {
        public static double Gauss(double x, double fwhm)
        {
            var sigma = Math.Abs(0.5 * fwhm / Math.Sqrt(2.0 * Math.Log(2.0)));
            return Math.Exp(-0.5 * Math.Pow(x / sigma, 2.0)) / Math.Sqrt(2.0 * Math.PI) / sigma;
        }

        public static double Lorentz(double x, double fwhm)
        {
            var gamma = Math.Abs(0.5 * fwhm);
            return 1.0 / (1.0 + Math.Pow(x / gamma, 2.0)) / Math.PI / gamma;
        }

        public static double StepAtan(double x, double w)
        {
            var gamma = Math.Abs(0.5 * w);
            return Math.Atan(x / gamma) / Math.PI + 0.5;
        }

        public static double PseudoVoigt(double x, double gauss_fwhm, double lorentz_fwhm, double gauss_ratio)
        {
            var eta = (gauss_ratio < 0.0) ? 0.0 : (gauss_ratio > 1.0) ? 1.0 : gauss_ratio;
            return eta * Gauss(x, gauss_fwhm) + (1.0 - eta) * Lorentz(x, lorentz_fwhm);
        }
    }
}
