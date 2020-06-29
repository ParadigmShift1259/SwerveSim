using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SwerveSim
{
	public class DoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double angle = 0.0;
			Double.TryParse((string)value, out angle);
			return angle % 360.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class Minus90Converter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double angle = 0.0;
			Double.TryParse((string)value, out angle);
			return (angle - 90.0) % 360.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class Plus180Converter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double angle = 0.0;
			Double.TryParse((string)value, out angle);
			return (angle + 180.0) % 360.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class NegateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double angle = 0.0;
			Double.TryParse((string)value, out angle);
			return (angle * -1.0) % 360.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	//public class RotationConverter : IValueConverter
	//{
	//	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		return (bool)value ? -45 : 135;	// CW -45 CCW 135
	//	}

	//	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private PointCollection m_translationVectorArrowPointPath = new PointCollection(10);
		private PointCollection m_rotationVectorArrowPointPath = new PointCollection(10);
		private double m_translationXcoordNW;
		private double m_translationYcoordNW;
		private double m_translationXcoordSW;
		private double m_translationYcoordSW;
		private double m_translationXcoordNE;
		private double m_translationYcoordNE;
		private double m_translationXcoordSE;
		private double m_translationYcoordSE;
		private PointCollection m_finalVectorArrowPointPathNW = new PointCollection(10);
		private PointCollection m_finalVectorArrowPointPathSW = new PointCollection(10);
		private PointCollection m_finalVectorArrowPointPathNE = new PointCollection(10);
		private PointCollection m_finalVectorArrowPointPathSE = new PointCollection(10);

		private Double m_scale = 3.5;
		private Double m_canvasSize = 130;

		private Double m_wheelDiameter = 16;
		private Double m_wheelWidth = 6;

		private Double m_lineLeftW = 30;
		private Double m_lineTopN = 40;
		private Double m_lineLeftE = 90;
		private Double m_lineTopS = 80;

		//private Double m_wheelLeftW = 22;
		//private Double m_wheelTopN = 37;
		//private Double m_wheelLeftE = 82;
		//private Double m_wheelTopS = 77;

		private Double m_arrowHeadSize = 1.0;
		private Double m_canvasFontSize = 10.0;
		private Double m_finalFontSize = 5.0;

		public MainWindow()
		{
			InitializeComponent();

			m_canvasSize *= m_scale;

			m_wheelDiameter *= m_scale;
			m_wheelWidth *= m_scale;

			m_lineLeftW *= m_scale;
			m_lineTopN *= m_scale;
			m_lineLeftE *= m_scale;
			m_lineTopS *= m_scale;

			//m_wheelLeftW *= m_scale;
			//m_wheelTopN *= m_scale;
			//m_wheelLeftE *= m_scale;
			//m_wheelTopS *= m_scale;

			m_arrowHeadSize *= m_scale;
			m_canvasFontSize *= m_scale;
			m_finalFontSize *= m_scale;

			Resources["wheelRadius"]    = m_wheelDiameter / 2;
			Resources["wheelDiameter"]  = m_wheelDiameter;
			Resources["wheelWidth"]     = m_wheelWidth;
			Resources["wheelHalfWidth"] = m_wheelWidth / 2;

			Resources["canvasSize"] = m_canvasSize;
			Resources["canvasFontSize"] = m_canvasFontSize;
			Resources["finalFontSize"] = m_finalFontSize;

			Resources["lineLeftW"] = m_lineLeftW;
			Resources["lineTopN"]  = m_lineTopN;
			Resources["lineLeftE"] = m_lineLeftE;
			Resources["lineTopS"]  = m_lineTopS;

			Resources["wheelLeftW"] = m_lineLeftW - m_wheelDiameter / 2;
			Resources["wheelTopN"]  = m_lineTopN  - m_wheelWidth / 2;
			Resources["wheelLeftE"] = m_lineLeftE - m_wheelDiameter / 2;
			Resources["wheelTopS"]  = m_lineTopS  - m_wheelWidth / 2;

			Update();
		}

		public void BuildVectorArrowPointPath(PointCollection pc, double arrowLen, string resourceName)
		{
			pc = new PointCollection(10)
			{
				new Point(0.0, 0.0),
				new Point(arrowLen, 0.0),
				new Point(arrowLen - m_arrowHeadSize, -1.0 * m_arrowHeadSize),
				new Point(arrowLen - m_arrowHeadSize, m_arrowHeadSize),
				new Point(arrowLen, 0.0)
			};

			Resources[resourceName] = pc;
		}

		public void CalcTranslationVectorAdditionCoords(double rotationArrowLen)
		{
			double x = Math.Sqrt((rotationArrowLen * rotationArrowLen) / 2);    // TODO only works for -45 and +135 degrees for now

			if (txtRotationAngle.Text == "135")
			{
				x *= -1.0;
			}

			m_translationXcoordNW = x;
			m_translationYcoordNW = x * -1.0;

			m_translationXcoordSW = x * -1.0;
			m_translationYcoordSW = x * -1.0;

			m_translationXcoordNE = x;
			m_translationYcoordNE = x;

			m_translationXcoordSE = x * -1.0;
			m_translationYcoordSE = x;

			Resources["translationXcoordNW"] = m_translationXcoordNW;
			Resources["translationYcoordNW"] = m_translationYcoordNW;
			
			Resources["translationXcoordSW"] = m_translationXcoordSW;
			Resources["translationYcoordSW"] = m_translationYcoordSW;
			
			Resources["translationXcoordNE"] = m_translationXcoordNE;
			Resources["translationYcoordNE"] = m_translationYcoordNE;
			
			Resources["translationXcoordSE"] = m_translationXcoordSE;
			Resources["translationYcoordSE"] = m_translationYcoordSE;
		}

		public void CalcFinalOutputVectorAdditionCoords(double translationArrowLen, double xCoord, double yCoord, PointCollection pc, string resourceSuffix)
		{
			double xFinal = translationArrowLen * Math.Cos(sldrTranslationAngle.Value * Math.PI / 180.0) + xCoord;
			double yFinal = translationArrowLen * Math.Sin(sldrTranslationAngle.Value * Math.PI / 180.0) + yCoord;

			double arrowLen = Math.Sqrt(xFinal * xFinal + yFinal * yFinal);
			BuildVectorArrowPointPath(pc, arrowLen, "finalVectorArrowPoints" + resourceSuffix);

			double angle = Math.Atan2(yFinal, xFinal);
			angle *= 180.0 / Math.PI;
			angle %= 360.0;
			Resources["finalAngle" + resourceSuffix] = angle;
			Resources["finalSpeed" + resourceSuffix] = arrowLen / m_scale / 3.2;
	}

	private void RadioButtonClockwise_Click(object sender, RoutedEventArgs e)
		{
			txtRotationAngle.Text = "-45";
			Update();
		}

		private void RadioButtonCounterClockwise_Click(object sender, RoutedEventArgs e)
		{
			txtRotationAngle.Text = "135";
			Update();
		}

		private void ValueChanged_sldrTranslationSpeed(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Update();
		}

		private void ValueChanged_sldrRotationSpeed(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Update();
		}

		private void ValueChanged_sldrTranslationAngle(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Update();
		}

		private void Update()
		{
			if (sldrRotationSpeed != null)
			{
				double rotationArrowLen = sldrRotationSpeed.Value * 3.1830988618379067153776752674503 * m_scale;    // Value on screen is radians/s, adust pixels by 20/2*pi
				double translationArrowLen = sldrTranslationSpeed.Value * 3.28084 * m_scale;    // Value on screen is m/s, but the pixels work out better in ft/s

				CalcTranslationVectorAdditionCoords(rotationArrowLen);

				BuildVectorArrowPointPath(m_rotationVectorArrowPointPath, rotationArrowLen, "rotationVectorArrowPoints");
				BuildVectorArrowPointPath(m_translationVectorArrowPointPath, translationArrowLen, "translationVectorArrowPoints");

				CalcFinalOutputVectorAdditionCoords(translationArrowLen, m_translationXcoordNW, m_translationYcoordNW, m_finalVectorArrowPointPathNW, "NW");
				CalcFinalOutputVectorAdditionCoords(translationArrowLen, m_translationXcoordSW, m_translationYcoordSW, m_finalVectorArrowPointPathSW, "SW");
				CalcFinalOutputVectorAdditionCoords(translationArrowLen, m_translationXcoordNE, m_translationYcoordNE, m_finalVectorArrowPointPathNE, "NE");
				CalcFinalOutputVectorAdditionCoords(translationArrowLen, m_translationXcoordSE, m_translationYcoordSE, m_finalVectorArrowPointPathSE, "SE");
			}
		}
	}
}
