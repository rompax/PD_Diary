﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PD_Diary.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DayPage : ContentPage
	{
		public DayPage ()
		{
			InitializeComponent ();
            ChosenDateLabel.Text = DateTime.Now.ToShortDateString();
        }
	}
}