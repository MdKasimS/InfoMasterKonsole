using InfoMasterKonsole;
using InfoMasterKonsole.Views;

AppManager app =
    AppManager.GetInstance();

MainMenuView mainMenu =
    new MainMenuView(app.CustomerService);

mainMenu.Show();