using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using PD_Diary.Services;
using PD_Diary.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;
using System.Threading;
using static Xamarin.Forms.Internals.GIFBitmap;

namespace PD_Diary.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DayPage : ContentPage
	{
		public DayPage ()
		{
			InitializeComponent ();
            DailyRecord dailyRecord = DailyRecord.GetDailyRecord(DateTime.Now);
            ApplyDailyRecord(dailyRecord);
        }
        public void ApplyDailyRecord(DailyRecord dailyRecord)
        {
            
            ChosenDateLabel.Text = String.Format("{0}.{1:00}.{2}", dailyRecord.Date.Day, dailyRecord.Date.Month, dailyRecord.Date.Year)  ;
            FillSummaryGrid(dailyRecord);
            FillMealGrid(dailyRecord);
        }

        private void FillMealGrid(DailyRecord dailyRecord)
        {
            int rowIdx = 0;
            foreach (Meal meal in dailyRecord.Meals.Values)
            {

                MealGrid.Children.Add(new Label() { Text = meal.Id.ToString() }, 0, rowIdx++);
                foreach (Consumption consumption in meal.Consumptions)
                {
                    MealGrid.Children.Add(new Label() { Text = "    " + App.Database.GetItemAsync(consumption.Id).Result.Name }, 0, rowIdx);
                    MealGrid.Children.Add(new Label() { Text = consumption.Weight.ToString(), HorizontalTextAlignment = TextAlignment.End }, 1, rowIdx);
                    MealGrid.Children.Add(new Label() { Text = "g" }, 2, rowIdx++);
                }
            }
        }

        private void FillSummaryGrid(DailyRecord dailyRecord)
        {
            Dictionary<ComponentType, double> components = new Dictionary<ComponentType, double>();
            foreach (Meal meal in dailyRecord.Meals.Values)
            {
                foreach (Consumption consumption in meal.Consumptions)
                {
                    Nutrient nutrient = App.Database.GetItem(consumption.Id);
                    foreach (Component component in nutrient.Components)
                    {
                        double addition = component.Per100gramm * consumption.Weight / 100.0;
                        if (components.ContainsKey(component.Id) )
                        {
                            components[component.Id] += addition;
                        }
                        else
                        {
                            components.Add(component.Id, addition);
                        }
                    }
                }
            }
            int rowIdx = 0;
            foreach (ComponentType componentType in components.Keys)
            {
                SummaryGrid.Children.Add(new Label() { Text = componentType.ToString() }, 0, rowIdx);
                SummaryGrid.Children.Add(new Label() { Text = components[componentType].ToString(), HorizontalTextAlignment = TextAlignment.End }, 1, rowIdx);
                SummaryGrid.Children.Add(new Label() { Text = componentType.GetUnits() }, 2, rowIdx);

                SKCanvasView skCanvas = new SKCanvasView() {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 20
                };
                skCanvas.PaintSurface += (sender, args) => {
                    var surface = args.Surface;
                    var surfaceWidth = args.Info.Width;
                    var surfaceHeight = args.Info.Height;
                    var r = Math.Min(surfaceHeight, surfaceWidth) / 2;

                    var canvas = surface.Canvas;

                    using (SKPaint paint = new SKPaint())
                    {
                        SKRect rect = new SKRect(surfaceWidth - 2 * r, 0, surfaceWidth, 2 * r);
                        paint.Shader = SKShader.CreateLinearGradient(
                            new SKPoint(rect.Left, rect.Top),
                            new SKPoint(rect.Right, rect.Bottom),
                            new SKColor[] { SKColors.Red, SKColors.Blue },
                            new float[] { 0, r },
                            SKShaderTileMode.Repeat);
                        canvas.DrawRect(rect, paint);
                    }

                    //canvas.DrawCircle(new SKPoint(surfaceWidth-r, r), 
                    //    r, 
                    //    new SKPaint() {
                    //        Color = Color.Red.ToSKColor(), IsAntialias= true, Style=SKPaintStyle.Fill
                    //    });
                    canvas.Flush();
                };

                SummaryGrid.Children.Add(skCanvas, 3, rowIdx);
                rowIdx++;
            }
        }
    }
}