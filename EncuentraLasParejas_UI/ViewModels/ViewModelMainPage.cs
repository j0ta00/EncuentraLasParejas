using EncuentraLasParejas_BL.Listados;
using EncuentraLasParejas_Entities;
using EncuentraLasParejas_UI.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace EncuentraLasParejas_UI.ViewModels
{
    public class ViewModelMainPage
    {
        public DelegateCommand<object> VerPuntuacion { get; set; }
        public DelegateCommand Jugar { get; set; }
        public DelegateCommand Retroceder { get; set; }
        private StackPanel StackPanelAuxiliar { get; set; }
        public ViewModelMainPage() {
            VerPuntuacion = new DelegateCommand<object>(mostrarPuntuacion_Execute);
            Jugar = new DelegateCommand(llevarAlTablero_Execute);
            Retroceder = new DelegateCommand(retroceder_Execute);
        }

        private void llevarAlTablero_Execute(){
            (Window.Current.Content as Frame).Navigate(typeof(Tablero));
        }
        private void retroceder_Execute()
        {
            
        }
        private void mostrarPuntuacion_Execute(object sender){
                Button btn = sender as Button;
                TextBlock txtScoreBoard = new TextBlock();
                txtScoreBoard.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                txtScoreBoard.FontWeight = FontWeights.Bold;
                txtScoreBoard.Text = "SCOREBOARD";
                txtScoreBoard.Margin = new Thickness(10);
                txtScoreBoard.FontSize = 40;
                txtScoreBoard.HorizontalAlignment = HorizontalAlignment.Center;
                ListView listaPuntuacion = new ListView();
                listaPuntuacion.ItemTemplate =((Window.Current.Content as Frame).Content as Page).Resources["scoreBoard"] as DataTemplate;
                listaPuntuacion.ItemsSource = ListadoPuntuacion_BL.getListadoCompletoPuntuacion();
                listaPuntuacion.IsHitTestVisible = false;
                listaPuntuacion.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                listaPuntuacion.BorderThickness = new Thickness(3);
                listaPuntuacion.CornerRadius = new CornerRadius(5);
                StackPanel stackPanel = VisualTreeHelper.GetParent(btn) as StackPanel;
                StackPanelAuxiliar=stackPanel;
                stackPanel.Children.Clear();
                stackPanel.Children.Add(txtScoreBoard);
                stackPanel.Children.Add(listaPuntuacion);
        }

    }
}
