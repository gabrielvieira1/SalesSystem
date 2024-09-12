using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Sales_system.Controls
{
  public sealed partial class LoadingControl : UserControl
  {
    public LoadingControl()
    {
      this.InitializeComponent();
    }

    // Método para exibir a animação de carregamento
    public void Show()
    {
      LoadingOverlay.Visibility = Visibility.Visible;
/*      FadeInStoryBoard.Begin();*/
    }

    // Método para ocultar a animação de carregamento
    public void Hide()
    {
      FadeOutStoryBoard.Completed += (s, e) =>
      {
        LoadingOverlay.Visibility = Visibility.Collapsed;
      };

      FadeOutStoryBoard.Begin();
    }
  }
}
