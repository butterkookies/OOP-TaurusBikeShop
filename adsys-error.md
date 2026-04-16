System.Windows.Markup.XamlParseException
  HResult=0x80131501
  Message='Initialization of 'System.Windows.Media.Imaging.BitmapImage' threw an exception.' Line number '908' and line position '58'.
  Source=PresentationFramework
  StackTrace:
   at System.Windows.Markup.XamlReader.RewrapException(Exception e, IXamlLineInfo lineInfo, Uri baseUri)
   at System.Windows.Markup.WpfXamlLoader.Load(XamlReader xamlReader, IXamlObjectWriterFactory writerFactory, Boolean skipJournaledProperties, Object rootObject, XamlObjectWriterSettings settings, Uri baseUri)
   at System.Windows.Markup.WpfXamlLoader.LoadBaml(XamlReader xamlReader, Boolean skipJournaledProperties, Object rootObject, XamlAccessLevel accessLevel, Uri baseUri)
   at System.Windows.Markup.XamlReader.LoadBaml(Stream stream, ParserContext parserContext, Object parent, Boolean closeStream)
   at System.Windows.Application.LoadComponent(Object component, Uri resourceLocator)
   at AdminSystem_v2.Views.OrdersView.InitializeComponent() in C:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\AdminSystem_v2\Views\OrdersView.xaml:line 1

  This exception was originally thrown at this call stack:
    [External Code]

Inner Exception 1:
InvalidOperationException: Property 'UriSource' or property 'StreamSource' must be set.
