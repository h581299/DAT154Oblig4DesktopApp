﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity.Core.Objects;
using DAT154Oblig4DesktopApp.Models;
using System.Xml.Linq;
using System.Diagnostics;

namespace DAT154Oblig4DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        Dat154oblig4Context dataEntities = new Dat154oblig4Context();

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
