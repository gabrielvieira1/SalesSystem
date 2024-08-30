using Connection;
using SamsungAccountLibrary.SAResponse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Sales_system.Util;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using SamsungAccountLibrary;

namespace Sales_system
{
  /// <summary>
  /// Provides application-specific behavior to supplement the default Application class.
  /// </summary>
  sealed partial class App : Application
  {
    private AppServiceConnection _appServiceConnection;
    private BackgroundTaskDeferral _inProcessAppServiceBackgroundDeferral;
    private AppServiceTriggerDetails _appService;

    protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
    {
      base.OnBackgroundActivated(args);
      IBackgroundTaskInstance taskInstance = args.TaskInstance;
      _appService = taskInstance.TriggerDetails as AppServiceTriggerDetails;
      _inProcessAppServiceBackgroundDeferral = taskInstance.GetDeferral(); //this is background defferal 
      taskInstance.Canceled += OnAppServicesCanceled;
      _appServiceConnection = _appService.AppServiceConnection;
      _appServiceConnection.RequestReceived += OnAppServiceRequestReceived;
      _appServiceConnection.ServiceClosed += AppServiceConnection_ServiceClosed;
    }

    private async void OnAppServiceRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    {
      AppServiceDeferral messageDeferral = args.GetDeferral();
      ValueSet message = args.Request.Message;

      ValueSet returnData = new ValueSet();
      SamsungAccountServiceResponseHandler samsungAccountServiceResponseHandler = SamsungAccountServiceResponseHandler.GetInstance();

      if (SamsungAccountServiceResponseHandler.GetInstance().IsSamsungAccountBroadcast(sender.AppServiceName))
      {
        // Return the data to the caller.
        // this step is optional, its just to notify that broadcast was successful
        // returnData["status"] = "received"; // you can send a simple acknowledgement that your app received the broadcast
        // await args.Request.SendResponseAsync(returnData); //now return this data to caller 

        messageDeferral.Complete(); // you already received the Broadcast message, you can call app service deferral to complete
        samsungAccountServiceResponseHandler.HandleServiceResponse(message);// this method will invoke signin/signout event

      }
      else
      {
        // do any app related work.
      }

    }

    private void OnAppServicesCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
      Debug.WriteLine($"App Service Canceled with Reason: {reason}");
      _inProcessAppServiceBackgroundDeferral.Complete();
    }

    private void AppServiceConnection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
    {
      Debug.WriteLine($"App Service Connection Closed with Status {args.Status}");
      _inProcessAppServiceBackgroundDeferral.Complete();
    }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
      this.InitializeComponent();
      this.Suspending += OnSuspending;
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
      SamsungAccountManager.SubscribeLogging();
      Frame rootFrame = Window.Current.Content as Frame;
      Storage.populateMetaData();

      // Do not repeat app initialization when the Window already has content,
      // just ensure that the window is active
      if (rootFrame == null)
      {
        // Create a Frame to act as the navigation context and navigate to the first page
        rootFrame = new Frame();

        rootFrame.NavigationFailed += OnNavigationFailed;

        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
        {
          //TODO: Load state from previously suspended application
        }

        // Place the frame in the current Window
        Window.Current.Content = rootFrame;
      }

      if (e.PrelaunchActivated == false)
      {
        if (rootFrame.Content == null)
        {
          // When the navigation stack isn't restored navigate to the first page,
          // configuring the new page by passing required information as a navigation
          // parameter
          rootFrame.Navigate(typeof(MainPage), e.Arguments);
        }
        // Ensure the current window is active
        Window.Current.Activate();
      }
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    protected override void OnActivated(IActivatedEventArgs args)
    {
      ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;
      Uri responseUri = protocolArgs.Uri;
      Debug.WriteLine(responseUri.ToString(), LoggingLevel.Information);
      SamsungAccountUriResponseParser responseParser = new SamsungAccountUriResponseParser(responseUri);
      MetaDataManager.GetInstance().GetSaSDKResponseListener().OnResponseReceived(responseParser.GetResponse());
      if (args != null)
      {
        Frame rootFrame = Window.Current.Content as Frame;

        // Do not repeat app initialization when the Window already has content, 
        // just ensure that the window is active

        if (rootFrame == null)
        {

          // Create a Frame to act as the navigation context and navigate to the first page
          rootFrame = new Frame();

          // Set the default language 
          rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
          rootFrame.NavigationFailed += OnNavigationFailed;

          // Place the frame in the current Window 
          Window.Current.Content = rootFrame;
        }

        if (rootFrame.Content == null)
        {

          // When the navigation stack isn't restored, navigate to the  
          // first page, configuring the new page by passing required  
          // information as a navigation parameter 

          rootFrame.Navigate(typeof(MainPage), null);
        }

        // Ensure the current window is active 
        Window.Current.Activate();

      }
    }
    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
      var deferral = e.SuspendingOperation.GetDeferral();
      //TODO: Save application state and stop any background activity
      deferral.Complete();
    }
  }
}
