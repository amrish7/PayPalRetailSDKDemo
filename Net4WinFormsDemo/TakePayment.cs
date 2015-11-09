using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;
using PayPalRetailSDK;

namespace Net4WinFormsDemo
{
    // ReSharper disable LocalizableElement
    public partial class MainForm : Form
    {
        private TransactionContext _context;
        private readonly Dispatcher _dispatcherUi;

        public MainForm()
        {
            InitializeComponent();
            _dispatcherUi = Dispatcher.CurrentDispatcher;
            RetailSDK.Initialize();
            messageTextBlock.Text = "SDK Initialized";
            InitializeMerchant();

            //Synchronizing amount field to terminal display
            txtAmount.TextChanged += (sender, args) =>
            {
                decimal amount;
                if (_context != null && _context.Invoice != null && decimal.TryParse(txtAmount.Text, out amount))
                {
                    //This would pump the updated total to the payment device
                    _context.Invoice.Items[0].UnitPrice = amount;
                }
            };
        }

        private void btnCharge_Click(object sender, EventArgs e)
        {
            messageTextBlock.Text = "Created Transaction Context";
            _context = _context ?? CreateTxContext(decimal.Parse(txtAmount.Text));
            _context.Completed += context_Completed;
            _context.Begin(true);
        }

        void context_Completed(TransactionContext sender, RetailSDKException error, TransactionRecord record)
        {
            _dispatcherUi.Invoke(new Action(() =>
            {
                messageTextBlock.Text = "Transaction Completed " + (error != null ? error.ToString() : "no error");
            }));
            _context = null;
        }

        private static TransactionContext CreateTxContext(decimal amount)
        {
            var invoice = new Invoice(null);
            invoice.AddItem("Amount", decimal.One, amount, "", "");
            return RetailSDK.CreateTransaction(invoice);
        }

        private async void InitializeMerchant()
        {
            try
            {
                var token = GetToken();
                var merchant = await RetailSDK.InitializeMerchant(token);
                _dispatcherUi.Invoke(new Action(() =>
                {
                    messageTextBlock.Text = "Merchant initialized: " + merchant.EmailAddress;
                }));
                _context = CreateTxContext(decimal.Parse(txtAmount.Text));
            }
            catch (Exception x)
            {
                _dispatcherUi.Invoke(new Action(() =>
                {
                    messageTextBlock.Text = "Merchant init failed: " + x;
                }));
            }
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
