using System.Xml.Linq;
using System;
using Autodesk.DataExchange.Models;
using Autodesk.DataExchange;
using Autodesk.DataExchange.Interface;
using Autodesk.DataExchange.Core.Interface;
using Autodesk.DataExchange.Extensions.HostingProvider;
using Autodesk.DataExchange.Extensions.Logging.File;
using Autodesk.DataExchange.Authentication;
using Autodesk.DataExchange.Core.Enums;
using Autodesk.DataExchange.DataModels;
using Autodesk.DataExchange.SchemaObjects.Units;
using Autodesk.DataExchange.Core.Models;

namespace aps_dx_sdk_form
{
	public partial class DXForm : Form
	{
		public DXForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private async void button1_Click(object sender, EventArgs e)
		{
			string AuthClientID = textBox_clientid.Text;
			string AuthClientSecret = textBox_clientsecret.Text;
			string AuthCallBack = "http://localhost:8080/";

			try
			{
				status_label.Text = "Creating Client...";
				IClient DXClient = CreateClient(AuthClientID, AuthClientSecret, AuthCallBack);

				string AppWorkspaceDirectory;
				ILogger Logger;
				CreateLogger(out AppWorkspaceDirectory, out Logger);

				status_label.Text = "Creating Auth...";
				IAuth Auth = CreateAuth(AuthClientID, AuthClientSecret, AuthCallBack, Logger);

				IHostingProvider HostingProvider = new ACC(Logger, () => Auth.GetAuthToken());

				GeometryConfiguration GeometryConfiguration = new GeometryConfiguration()
				{
					STEPProtocol = STEPProtocol.ConfigurationControlledDesign,
					STEPTolerance = 0.001
				};
				ExchangeDetails ExchangeDetails = await CreateExchange(DXClient, HostingProvider);

				//Defining the render style
				RGBA rgba = new RGBA(215, 0, 0, 120);
				RenderStyle CommonRenderStyle = new RenderStyle("Red", rgba, 1);

				status_label.Text = "Creating Element...";
				// Create ElementDataModel wrapper
				ElementDataModel ExchangeDataWrapper;
				Element Element;
				CreateElement(DXClient, out ExchangeDataWrapper, out Element);

				status_label.Text = "Creating Geometry...";
				// Create Geometry
				string Path = System.IO.Path.Combine(AppWorkspaceDirectory, "column.ifc");
				Path = LoadGeometryFile(Path);

				//CreateGeometry is a static method, it can't be created from an instance of ElementDataModel
				CreateGeometry(CommonRenderStyle, ExchangeDataWrapper, Element, Path);

				var ExchangeIdentifier = new DataExchangeIdentifier
				{
					CollectionId = ExchangeDetails.CollectionID,
					ExchangeId = ExchangeDetails.ExchangeID,
					HubId = ExchangeDetails.HubId
				};

				status_label.Text = "Creating Data Exchange...";
				//publish the exchange
				await DXClient.SyncExchangeDataAsync(ExchangeIdentifier, ExchangeDataWrapper.ExchangeData);

				status_label.Text = "Generating viewable...";
				//Generate viewable
				await DXClient.GenerateViewableAsync(ExchangeIdentifier.ExchangeId, ExchangeIdentifier.CollectionId);

				status_label.Text = "Data Exchange created successfully";
			}
			catch (Exception ex)
			{
				status_label.Text = ex.Message;
			}

			//wait 3 seconds
			await Task.Delay(3000);
			status_label.Text = "";
		}

		private static void CreateGeometry(RenderStyle CommonRenderStyle, ElementDataModel ExchangeDataWrapper, Element Element, string Path)
		{
			Geometry Geometry = ElementDataModel.CreateGeometry(new GeometryProperties(Path, CommonRenderStyle));

			// Map elements and geometry.        
			var ElementGeometry = new List<ElementGeometry> { Geometry };
			ExchangeDataWrapper.SetElementGeometryByElement(Element, ElementGeometry);
		}

		private static string LoadGeometryFile(string Path)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//files with .step, .ifc, and.obj extensions
				openFileDialog.Filter = "Geometry Files (*.step;*.ifc;*.obj)|*.step;*.ifc;*.obj";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					Path = openFileDialog.FileName;
				}
			}

			return Path;
		}

		private static void CreateElement(IClient DXClient, out ElementDataModel ExchangeDataWrapper, out Element Element)
		{
			ExchangeDataWrapper = ElementDataModel.Create(DXClient);
			ExchangeDataWrapper.SetViewableWorldCoordinates(new ViewableWorldCoordinates()
			{
				//default value will be UP=0 0 1, Front=0 -1 0, North=0 1 0
				UP = new Autodesk.GeometryPrimitives.Math.Vector3d(1, 0, 0),
				Front = new Autodesk.GeometryPrimitives.Math.Vector3d(0, 0, 1),
				North = new Autodesk.GeometryPrimitives.Math.Vector3d(0, 0, -1)
			});

			// Adds elements to the exchange (child elements are not present)
			Element = ExchangeDataWrapper.AddElement(new ElementProperties("1234", "Column", "Structural Column", "Column", "Steel Column")
			{
				LengthUnit = UnitFactory.MilliMeter,
				DisplayLengthUnit = UnitFactory.MilliMeter
			});
		}

		private async Task<ExchangeDetails> CreateExchange(IClient DXClient, IHostingProvider HostingProvider)
		{
			ExchangeCreateRequestACC ExchangeCreateRequest = new ExchangeCreateRequestACC()
			{
				Host = HostingProvider,
				Contract = new Autodesk.DataExchange.ContractProvider.ContractProvider(),
				Description = "Sample Data Exchange created from SDK",
				FileName = textBox_filename.Text,
				HubId = textBox_hubid.Text,
				ACCFolderURN = textBox_folderurn.Text,
				ProjectId = textBox_projectid.Text,
				ProjectType = ProjectType.ACC,
				Region = "US"
			};

			ExchangeDetails ExchangeDetails = await DXClient.CreateExchangeAsync(ExchangeCreateRequest);
			return ExchangeDetails;
		}

		private static IAuth CreateAuth(string AuthClientID, string AuthClientSecret, string AuthCallBack, ILogger Logger)
		{
			var AuthOption = new AuthOptions
			{
				ClientId = AuthClientID,
				ClientSecret = AuthClientSecret,
				CallBack = AuthCallBack,
				Logger = Logger
			};
			IAuth Auth = new Auth(AuthOption);
			return Auth;
		}

		private static void CreateLogger(out string AppWorkspaceDirectory, out ILogger Logger)
		{
			AppWorkspaceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string AppBasePath = System.IO.Path.Combine(AppWorkspaceDirectory, "aps-ds-sdk-form");
			var LogDirectoryPath = System.IO.Path.Combine(AppBasePath, "logs");
			Logger = new Log(LogDirectoryPath);
		}

		private static IClient CreateClient(string AuthClientID, string AuthClientSecret, string AuthCallBack)
		{
			SDKOptionsDefaultSetup SdkOptionsDefaultSetup = new SDKOptionsDefaultSetup()
			{
				ClientId = AuthClientID,
				ClientSecret = AuthClientSecret,
				CallBack = AuthCallBack,
				ConnectorName = "Sample-Connector",
				ConnectorVersion = "1.0.0",
				HostApplicationName = "Revit",
				HostApplicationVersion = "2023.0.1"
			};

			// Synchronous
			IClient DXClient = new Client(SdkOptionsDefaultSetup);
			return DXClient;
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}
}
