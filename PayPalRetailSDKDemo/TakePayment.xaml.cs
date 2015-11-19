using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using PayPalRetailSDK;

namespace Net4WPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TakePayment : Window
    {
        private TransactionContext _context;
        private string merchantStatus;

        public TakePayment()
        {
            InitializeComponent();
            RetailSDK.SetNetworkHandler(new CustomNetworkHandler());
            RetailSDK.Initialize();
            ChargeButton.IsEnabled = false;
            MessageTextBlock.Text = "SDK Initialized";
            InitializeMerchant();

            //Synchronizing amount field to terminal display
            amountField.TextChanged += (sender, args) =>
            {
                decimal amount;
                if (_context != null && _context.Invoice != null && decimal.TryParse(amountField.Text, out amount))
                {
                    //This would pump the updated total to the payment device
                    _context.Invoice.Items[0].UnitPrice = amount;
                }
            };

            RetailSDK.DeviceDiscovered += (sender, device) =>
            {
                device.Connected += pd =>
                {
                    try
                    {
                        var deviceId = pd.Id;
                        Dispatcher.Invoke(DispatcherPriority.Input, TimeSpan.MaxValue, new Action(() =>
                        {
                            MessageTextBlock.Text = string.Format("{0}Connected to {1}", merchantStatus, deviceId);
                            ChargeButton.IsEnabled = true;
                            const decimal serviceFee = 2.7m;
                            var total = decimal.Parse(amountField.Text);
                            _context = CreateTxContext(total);
                            _context.TotalDisplayFooter = string.Format("\n+${0}% fee\n${1}", serviceFee, decimal.Round(total * (1 + serviceFee/100), 2));
                        }));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                };
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageTextBlock.Text = "Created Transaction Context";
            _context = _context ?? CreateTxContext(decimal.Parse(amountField.Text));
            _context.Completed += context_Completed;
            _context.Begin(true);
        }

        void context_Completed(TransactionContext sender, RetailSDKException error, TransactionRecord record)
        {
            var errorMsg = (error != null ? error.ToString() : "no error");
            Dispatcher.Invoke(new Action(() =>
            {
                MessageTextBlock.Text = "Transaction Completed " + errorMsg;
            }));
            _context = null;
        }

        private async void InitializeMerchant()
        {
            try
            {
                var token = GetToken();
                var merchant = await RetailSDK.InitializeMerchant(token);
                var emailId = merchant.EmailAddress;
                Dispatcher.Invoke(DispatcherPriority.Input, TimeSpan.MaxValue, new Action(() =>
                {
                    merchantStatus = "Merchant initialized: " + emailId + "\n";
                    MessageTextBlock.Text = string.Format("{0}Looking for devices", merchantStatus);
                }));
            }
            catch (Exception x)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MessageTextBlock.Text = "Merchant init failed: " + x;
                }));
            }
        }

        private static TransactionContext CreateTxContext(decimal amount)
        {
            var invoice = new Invoice(null);
            invoice.AddItem("Amount", decimal.One, amount, "", "");
            return RetailSDK.CreateTransaction(invoice);
        }

        private string GetToken()
        {
            const string tokenPath = @".\testToken.txt";
            var token = new StreamReader(tokenPath).ReadToEnd();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("Invalid token");
            }
            return token;
        }
    }
}
