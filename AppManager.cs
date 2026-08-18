using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Repositories;
using InfoMasterKonsole.Services;
using InfoMasterKonsole.Validation;

namespace InfoMasterKonsole;

public class AppManager
{
    private static AppManager? instance;

    public ICustomerRepository CustomerRepository { get; private set; }

    public CustomerValidator CustomerValidator { get; private set; }

    public ICustomerService CustomerService { get; private set; }

    private AppManager()
    {
        CustomerRepository =
    new CustomerRepository();

        CustomerValidator =
            new CustomerValidator(
                CustomerRepository);

        IDataSerializer jsonSerializer =
            new JsonDataSerializer();

        IDataSerializer xmlSerializer =
            new XmlDataSerializer();

        CustomerService =
            new CustomerService(
                CustomerRepository,
                CustomerValidator,
                jsonSerializer,
                xmlSerializer);
    }

    public static AppManager GetInstance()
    {
        if (instance == null)
        {
            instance = new AppManager();
        }

        return instance;
    }
}