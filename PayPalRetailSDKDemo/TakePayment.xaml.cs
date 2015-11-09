using System;
using System.IO;
using System.Windows;
using PayPalRetailSDK;

namespace Net4WPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TakePayment : Window
    {
        private TransactionContext _context;

        public TakePayment()
        {
            InitializeComponent();
            RetailSDK.Initialize();
            messageTextBlock.Text = "SDK Initialized.";

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            messageTextBlock.Text = "Created Transaction Context";
            _context = _context ?? CreateTxContext(decimal.Parse(amountField.Text));
            _context.Completed += context_Completed;
            _context.Begin(true);
        }

        void context_Completed(TransactionContext sender, RetailSDKException error, TransactionRecord record)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                messageTextBlock.Text = "Transaction Completed " + (error != null ? error.ToString() : "no error");
            }));
            _context = null;
        }

        private async void InitializeMerchant()
        {
            try
            {
                var token = GetToken();
                var merchant = await RetailSDK.InitializeMerchant(token);
                Dispatcher.Invoke(new Action(() =>
                {
                    messageTextBlock.Text = "Merchant initialized: " + merchant.EmailAddress;
                }));
                _context = CreateTxContext(decimal.Parse(amountField.Text));
            }
            catch (Exception x)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    messageTextBlock.Text = "Merchant init failed: " + x;
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
