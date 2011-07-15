// AForge Image Processing Library
// AForge.NET framework
//
// Copyright © Andrew Kirillov, 2005-2009
// andrew.kirillov@aforgenet.com
//

namespace AForge.Imaging.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using AForge;

    /// <summary>
    /// Brightness adjusting in HSL color space.
    /// </summary>
    /// 
    /// <remarks><para>The filter operates in <b>HSL</b> color space and adjusts
    /// pixels' brightness value using luminance value of HSL color space, increasing it
    /// or decreasing by specified percentage. The filters is based on <see cref="HSLLinear"/>
    /// filter, passing work to it after recalculating brightness
    /// <see cref="AdjustValue">adjust value</see> to input/output ranges of the
    /// <see cref="HSLLinear"/> filter.</para>
    /// 
    /// <para>The filter accepts 24 and 32 bpp color images for processing.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // create filter
    /// BrightnessCorrection filter = new BrightnessCorrection( -0.15 );
    /// // apply the filter
    /// filter.ApplyInPlace( image );
    /// </code>
    /// 
    /// <para><b>Initial image:</b></para>
    /// <img src="img/imaging/sample1.jpg" width="480" height="361" />
    /// <para><b>Result image:</b></para>
    /// <img src="img/imaging/brightness_correction.jpg" width="480" height="361" />
    /// </remarks>
    /// 
    public class BrightnessCorrection : BaseInPlacePartialFilter
    {
        private HSLLinear baseFilter = new HSLLinear( );
        private double adjustValue;	// [-1, 1]
        private bool keepBW = false;
        private bool keepGray = false;
        private int grayTolerance = 0;

        /// <summary>
        /// Brightness adjust value, [-1, 1].
        /// </summary>
        /// 
        /// <remarks>Default value is set to <b>0.1</b>, which corresponds to increasing
        /// brightness by 10%.</remarks>
        ///
        public double AdjustValue
        {
            get { return adjustValue; }
            set
            {
                adjustValue = Math.Max( -1.0, Math.Min( 1.0, value ) );

                // create luminance filter
                if ( adjustValue > 0 )
                {
                    baseFilter.InLuminance  = new DoubleRange( 0.0, 1.0 - adjustValue );
                    baseFilter.OutLuminance = new DoubleRange( adjustValue, 1.0 );
                }
                else
                {
                    baseFilter.InLuminance  = new DoubleRange( -adjustValue, 1.0 );
                    baseFilter.OutLuminance = new DoubleRange( 0.0, 1.0 + adjustValue );
                }
            }
        }

        /// <summary>
        /// Leave blacks and whites alone.
        /// </summary>
        /// 
        /// <remarks><para>Default value is set to <see langword="false"/>.</para></remarks>
        /// 
        public bool KeepBW
        {
            get { return keepBW; }
            set { keepBW = value; }
        }

        /// <summary>
        /// Leave grays alone.
        /// </summary>
        /// 
        /// <remarks><para>Default value is set to <see langword="false"/>.</para></remarks>
        /// 
        public bool KeepGray
        {
            get { return keepGray; }
            set { keepGray = value; }
        }

        /// <summary>
        /// Gray Tolerance
        /// </summary>
        /// 
        /// <remarks><para>Default value is set to 0.</para></remarks>
        /// 
        public int GrayTolerance
        {
            get { return grayTolerance; }
            set { grayTolerance = value; }
        }

        // format translation dictionary
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>( );

        /// <summary>
        /// Format translations dictionary.
        /// </summary>
        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrightnessCorrection"/> class.
        /// </summary>
        /// 
        public BrightnessCorrection( ) : this( 0.1 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrightnessCorrection"/> class.
        /// </summary>
        /// 
        /// <param name="adjustValue">Brightness adjust value.</param>
        /// 
        public BrightnessCorrection( double adjustValue )
        {
            AdjustValue = adjustValue;

            formatTranslations[PixelFormat.Format24bppRgb]  = PixelFormat.Format24bppRgb;
            formatTranslations[PixelFormat.Format32bppRgb]  = PixelFormat.Format32bppRgb;
            formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrightnessCorrection"/> class.
        /// </summary>
        /// 
        /// <param name="adjustValue">Brightness adjust value.</param>
        /// <param name="keepBW">Keep blacks and whites alone.</param>
        /// <param name="keepGray">Keep grays alone.</param>
        /// <param name="grayTolerance">Gray Tolerance range to leave untouched.</param>
        /// 
        public BrightnessCorrection(double adjustValue, bool keepBW, bool keepGray, int grayTolerance)
        {
            AdjustValue = adjustValue;
            this.keepBW = keepBW;
            this.keepGray = keepGray;
            this.grayTolerance = grayTolerance;
            baseFilter.KeepBW = keepBW;
            baseFilter.KeepGray = keepGray;
            baseFilter.GrayTolerance = grayTolerance;

            formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
            formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
            formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
        }

        /// <summary>
        /// Process the filter on the specified image.
        /// </summary>
        /// 
        /// <param name="image">Source image data.</param>
        /// <param name="rect">Image rectangle for processing by the filter.</param>
        ///
        protected override unsafe void ProcessFilter( UnmanagedImage image, Rectangle rect )
        {
            baseFilter.ApplyInPlace( image, rect );
        }
    }
}
