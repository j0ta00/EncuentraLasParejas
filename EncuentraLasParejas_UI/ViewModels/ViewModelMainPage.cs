using EncuentraLasParejas_BL.Listados;
using EncuentraLasParejas_Entities;
using EncuentraLasParejas_UI.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        #region propiedades
        public DelegateCommand<object> VerPuntuacion { get; set; }
        public DelegateCommand Jugar { get; set; }
        public DelegateCommand<object> Retroceder { get; set; }
        private StackPanel StackPanelAuxiliar { get; set; }
        #endregion
        #region constructor
        public ViewModelMainPage() {
            VerPuntuacion = new DelegateCommand<object>(mostrarPuntuacion_Execute);
            Jugar = new DelegateCommand(llevarAlTablero_Execute);
            Retroceder = new DelegateCommand<object>(retroceder_Execute);
        }
        #endregion
        #region metodos y commands
        /// <summary>
        /// Command asociado a un botón el cual lleva a una vista de juego una vez se haya clicado en dicho botón
        /// </summary>
        private void llevarAlTablero_Execute(){
            (Window.Current.Content as Frame).Navigate(typeof(Tablero));
        }
        /// <summary>
        /// Modifica los elementos visuales de la página para volver al menú principal
        /// </summary>
        /// <param name="sender">object sender</param>
        private void retroceder_Execute(object sender)
        {
            List<UIElement> lista;
            StackPanel stackPanel = VisualTreeHelper.GetParent(sender as AppBarButton) as StackPanel;
            stackPanel.Children.Clear();
            lista = StackPanelAuxiliar.Children.ToList();
            StackPanelAuxiliar.Children.Clear();
            lista.ForEach(elementoVisual => stackPanel.Children.Add(elementoVisual));
        }
        /// <summary>
        /// Este macro método es basicamente una forma de complicarme la vida, ya que literalmente podría ser una typeof a una página, pero con el fin
        /// de querer probar el tema de los recursos, los commands parameters y aprender un poco sobre los arboles de los elementos visuales, me propuse ahorrarme esa página y modificar
        ///la main page para que cuando se clicase, de forma dinámica se actualizase la vista sin necesidad de mandarte a otra página, literalmente no tiene otra motivación más que
        ///el puro aprendizaje, de hecho me hubiese ahorrado una cantidad de horas tremenda si hubiera optado por hacerlo en una nueva página.
        ///En definitiva este método me ha llevado muchas horas, uso mil cosas muy interesantes que no había hecho desde código c# solo desde xaml y ha sido muy divertido a la par
        /// que frustrante, pues constantemente los elementos desaparecian sin motivo o al volver a la menu principal aparecian de nuevo y se superponian, entre otros muchos más errores.
        /// 
        /// </summary>
        /// <param name="sender">object sender</param>
        private void mostrarPuntuacion_Execute(object sender){
                Button btn = sender as Button;
                RelativePanel relativePanel = new RelativePanel();
                StackPanel stackPanel;
                ScrollViewer scrViewer = new ScrollViewer();
                scrViewer.IsHorizontalRailEnabled = true;
                scrViewer.HorizontalScrollMode = ScrollMode.Enabled;
                scrViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                List<UIElement> lista;
                TextBlock txtScoreBoard;
                ListView listaPuntuacion;
                AppBarButton retroceder = new AppBarButton ();
                retroceder.Icon = new SymbolIcon(Symbol.Back);
                retroceder.Foreground= new SolidColorBrush(Windows.UI.Colors.White);
                retroceder.Command = Retroceder;
                retroceder.CommandParameter = retroceder;
                retroceder.Margin = new Thickness(0,0,0,-150);
                retroceder.HorizontalAlignment = HorizontalAlignment.Left;
                txtScoreBoard = new TextBlock();
                txtScoreBoard.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                txtScoreBoard.FontWeight = FontWeights.Bold;
                txtScoreBoard.Text = "SCOREBOARD";
                txtScoreBoard.Margin = new Thickness(10);
                txtScoreBoard.FontSize = 40;
                txtScoreBoard.HorizontalAlignment = HorizontalAlignment.Center;
                listaPuntuacion = new ListView();
                listaPuntuacion.ItemTemplate =((Window.Current.Content as Frame).Content as Page).Resources["scoreBoard"] as DataTemplate;/*aquí le asigno la key de mi recurso, me ha costado la vida encontrar que esto es lo que debía hacer,
                                                                                                                                           porqué claro suena muy logico hacer el datatemplate como recurso y asignarselo al listview, pero suena lógico
                                                                                                                                           cuando sabes bien la definición de recurso y cuando ves lo que tienes que hacer, porqué a esta idea
                                                                                                                                           llegue de milagro tras buscar de mil formas como hacer un data template desde código y claro, yo no me imaginaba
                                                                                                                                           que podía hacer el recurso en el xaml y usarlo fuera de el para asignarselo así, por lo cual cuando lo vi y lo probe,
                                                                                                                                            comence a entender de verdad las muchas funcionalidades de los recursos.
                                                                                                                                           */
            try {
                listaPuntuacion.ItemsSource = ListadoPuntuacion_BL.getListadoCompletoPuntuacion().OrderBy(puntuacion=>puntuacion.Tiempo);
            }
            catch (SqlException){
                mostrarContentDialogErrorSql();
            }
                listaPuntuacion.IsHitTestVisible = false;
                listaPuntuacion.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                listaPuntuacion.BorderThickness = new Thickness(3);
                listaPuntuacion.CornerRadius = new CornerRadius(5);
                stackPanel = VisualTreeHelper.GetParent(btn) as StackPanel;
                relativePanel= VisualTreeHelper.GetParent(stackPanel) as RelativePanel;
                if(((Window.Current.Content as Frame).Content as Page).Content as ScrollViewer is null){//así evito que cuando se vuelva a entrar no se vuelva a asignar el scrollviewer cuando ya lo tiene
                ((Window.Current.Content as Frame).Content as Page).Content = scrViewer;
                scrViewer.Content = relativePanel;
                }
                StackPanelAuxiliar = new StackPanel();
                lista=stackPanel.Children.ToList();
                stackPanel.Children.Clear();
                stackPanel.Children.Add(retroceder);
                stackPanel.Children.Add(txtScoreBoard);
                stackPanel.Children.Add(listaPuntuacion);
                lista.ForEach(elementoVisual => StackPanelAuxiliar.Children.Add(elementoVisual));    
         }
        /// <summary>
        /// Muestra un dialogo alertando al usuario que ha sucedido un error al acceder a la base de datos
        /// </summary>
        private async void mostrarContentDialogErrorSql()
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "An Sql Error Ocurred",
                Content = "Check your connection or the data of the connections strings",
                CloseButtonText = "Ok"
            };
            await dialog.ShowAsync();
        }
        #endregion
    }
}
