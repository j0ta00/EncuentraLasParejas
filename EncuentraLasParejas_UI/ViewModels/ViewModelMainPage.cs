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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Binding = Windows.UI.Xaml.Data.Binding;

namespace EncuentraLasParejas_UI.ViewModels
{
    public class ViewModelMainPage
    {
        public DelegateCommand<object> VerPuntuacion { get; set; }
        public DelegateCommand Jugar { get; set; }
        public DelegateCommand<object> Retroceder { get; set; }
        private StackPanel StackPanelAuxiliar { get; set; }
        public ViewModelMainPage() {
            VerPuntuacion = new DelegateCommand<object>(mostrarPuntuacion_Execute);
            Jugar = new DelegateCommand(llevarAlTablero_Execute);
            Retroceder = new DelegateCommand<object>(retroceder_Execute);
        }

        private void llevarAlTablero_Execute(){
            (Window.Current.Content as Frame).Navigate(typeof(Tablero));
        }
        private void retroceder_Execute(object sender)
        {
            List<UIElement> lista;
            StackPanel stackPanel = VisualTreeHelper.GetParent(sender as AppBarButton) as StackPanel;
            stackPanel.Children.Clear();
            lista = StackPanelAuxiliar.Children.ToList();
            StackPanelAuxiliar.Children.Clear();
            lista.ForEach(elementoVisual => stackPanel.Children.Add(elementoVisual));
        }
        private void mostrarPuntuacion_Execute(object sender){
                Button btn = sender as Button;
                StackPanel stackPanel;
                List<UIElement> lista;
                TextBlock txtScoreBoard;
                ListView listaPuntuacion;
                AppBarButton retroceder = new AppBarButton ();
                retroceder.Icon = new SymbolIcon(Symbol.Back);
                retroceder.Foreground= new SolidColorBrush(Windows.UI.Colors.White);
                retroceder.Command = Retroceder;
                retroceder.CommandParameter = retroceder;
                retroceder.HorizontalAlignment = HorizontalAlignment.Left;
                txtScoreBoard = new TextBlock();
                txtScoreBoard.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                txtScoreBoard.FontWeight = FontWeights.Bold;
                txtScoreBoard.Text = "SCOREBOARD";
                txtScoreBoard.Margin = new Thickness(10);
                txtScoreBoard.FontSize = 40;
                txtScoreBoard.HorizontalAlignment = HorizontalAlignment.Center;
                listaPuntuacion = new ListView();
                listaPuntuacion.ItemTemplate =((Window.Current.Content as Frame).Content as Page).Resources["scoreBoard"] as DataTemplate;
                listaPuntuacion.ItemsSource = ListadoPuntuacion_BL.getListadoCompletoPuntuacion();
                listaPuntuacion.IsHitTestVisible = false;
                listaPuntuacion.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                listaPuntuacion.BorderThickness = new Thickness(3);
                listaPuntuacion.CornerRadius = new CornerRadius(5);
                stackPanel = VisualTreeHelper.GetParent(btn) as StackPanel;
                StackPanelAuxiliar = new StackPanel();
                lista=stackPanel.Children.ToList();
                stackPanel.Children.Clear();
                stackPanel.Children.Add(retroceder);
                stackPanel.Children.Add(txtScoreBoard);
                stackPanel.Children.Add(listaPuntuacion);
                lista.ForEach(elementoVisual => StackPanelAuxiliar.Children.Add(elementoVisual));    
         }
    }
}
