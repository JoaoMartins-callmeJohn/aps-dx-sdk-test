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
	public partial class Form1 : Form
	{
		public Form1()
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

			var AppWorkspaceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string AppBasePath = System.IO.Path.Combine(AppWorkspaceDirectory, "aps-ds-sdk-form");
			var LogDirectoryPath = System.IO.Path.Combine(AppBasePath, "logs");
			ILogger Logger = new Log(LogDirectoryPath);

			var AuthOption = new AuthOptions
			{
				ClientId = AuthClientID,
				ClientSecret = AuthClientSecret,
				CallBack = AuthCallBack,
				Logger = Logger
			};
			IAuth Auth = new Auth(AuthOption);

			IHostingProvider HostingProvider = new ACC(Logger, () => Auth.GetAuthToken());

			GeometryConfiguration GeometryConfiguration = new GeometryConfiguration()
			{
				STEPProtocol = STEPProtocol.ConfigurationControlledDesign,
				STEPTolerance = 0.001
			};

			var ExchangeCreateRequest = new ExchangeCreateRequestACC()
			{
				Host = HostingProvider,
				Contract = new Autodesk.DataExchange.ContractProvider.ContractProvider(),
				Description = string.Empty,
				FileName = textBox_filename.Text,
				HubId = textBox_hubid.Text,
				ACCFolderURN = textBox_folderurn.Text,
				ProjectId = textBox_projectid.Text,
				ProjectType = ProjectType.ACC,
				Region = "US"
			};

			ExchangeDetails ExchangeDetails = await DXClient.CreateExchangeAsync(ExchangeCreateRequest);

			RGBA rgba = new RGBA(215, 0, 0, 120);
			RenderStyle CommonRenderStyle = new RenderStyle("Red", rgba, 1);

			// Create ElementDataModel wrapper
			ElementDataModel ExchangeDataWrapper = ElementDataModel.Create(DXClient);

			ExchangeDataWrapper.SetViewableWorldCoordinates(new ViewableWorldCoordinates()
			{
				//default value will be UP=0 0 1, Front=0 -1 0, North=0 1 0
				UP = new Autodesk.GeometryPrimitives.Math.Vector3d(1, 0, 0),
				Front = new Autodesk.GeometryPrimitives.Math.Vector3d(0, 0, 1),
				North = new Autodesk.GeometryPrimitives.Math.Vector3d(0, 0, -1)
			});

			// Adds elements to the exchange (child elements are not present)
			var Element = ExchangeDataWrapper.AddElement(new ElementProperties("1234", "Towers", "Mass", "Cylinder", "Four Cylinders")
			{
				LengthUnit = UnitFactory.MilliMeter,
				DisplayLengthUnit = UnitFactory.MilliMeter
			});

			// Create Geometry
			var Path = System.IO.Path.Combine(AppWorkspaceDirectory, "sample.ifc");
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "geometry files *.step|*.ifc|*.obj";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					Path = openFileDialog.FileName;
				}
			}

			
			//CreateGeometry is a static method, it can't be created from an instance of ElementDataModel
			Geometry Geometry = ElementDataModel.CreateGeometry(new GeometryProperties(Path, CommonRenderStyle));

			// Map elements and geometry.        
			var ElementGeometry = new List<ElementGeometry> { Geometry };
			ExchangeDataWrapper.SetElementGeometryByElement(Element, ElementGeometry);

			var ExchangeIdentifier = new DataExchangeIdentifier
			{
				CollectionId = ExchangeDetails.CollectionID,
				ExchangeId = ExchangeDetails.ExchangeID,
				HubId = ExchangeDetails.HubId
			};

			//publish the exchange
			await DXClient.SyncExchangeDataAsync(ExchangeIdentifier, ExchangeDataWrapper.ExchangeData);
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void textBox3_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
