using ImprovedPicpay.Enums;
using ImprovedPicpay.Events;
using ImprovedPicpay.Helpers;
using  ImprovedPicpay.Primitives;

namespace ImprovedPicpay.Entities
{
    public class User : AggregateRoot
    {
        private readonly List<Transaction> _transactions = new();
        public User(string id,
                    string userFirstName,
                    string userMiddleName,
                    string userLastName,
                    string email,
                    string password,
                    string userType,
                    string document,
                    decimal balance) : base(id)
        {
            UserFirstName = userFirstName;
            UserMiddleName = userMiddleName;
            UserLastName = userLastName;
            Email = email;
            Password = password;
            DateOfRegistration = DateTime.UtcNow;
            IsLocked = false;
            UserType = userType;
            Document = document;
            Balance = balance;
        }

        /// <summary>
        /// User's first name.
        /// </summary>
        public string UserFirstName { get; private set; }

        /// <summary>
        /// User's middle name.
        /// </summary>
        public string UserMiddleName { get; private set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        public string UserLastName { get; private set; }

        /// <summary>
        /// User's email address that will be used to log in.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// User's password that will be used to log in.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Date and time when the user was registered to the application.
        /// </summary>
        public DateTime DateOfRegistration { get; private set; }

        /// <summary>
        /// Specifies if the users is locked or not.
        /// </summary>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Specifies the type of user.
        /// </summary>
        public string UserType { get; private set; }

        /// <summary>
        /// User document number
        /// </summary>
        public string Document { get; private set; }

        /// <summary>
        /// User's balance
        /// </summary>
        public decimal Balance { get; set; }

        public IReadOnlyCollection<Transaction> Transactions => _transactions;

        public Transaction Transfer(User receiver, decimal amount)
        {
            if (Id == receiver.Id)
            {
                return ServiceResponse.Failure<Transaction>(
                    new Error("CreateTransaction.UserTypeCannotSendMoney", "Can't send money to yourself."));
            }

            bool senderIsShopkeeper = UserType.Equals(UserTypes.Shopkeeper.ToString());
            if (senderIsShopkeeper)
            {
                return ServiceResponse.Failure<Transaction>(
                    new Error("CreateTransaction.UserTypeCannotSendMoney", "Shopkeeper's can only receive money."));
            }

            bool haveBalance = Balance >= amount;
            if (!haveBalance)
            {
                return ServiceResponse.Failure<Transaction>(
                    new Error("CreateTransaction.LowBalance", "You don't have enough balance to perform this operation."));
            }

            Balance -= amount;

            var transaction = new Transaction(Guid.NewGuid().ToString(), Id, receiver.Id, amount);

            _transactions.Add(transaction);

            return transaction;
        }

        public void IncreaseBalance(decimal amount) => Balance += amount;
    }
}
