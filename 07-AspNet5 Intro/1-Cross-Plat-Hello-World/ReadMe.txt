Demo: ASP.NET 5 Cross-Platform Hello World

In this demo we'll deploy a console app to Mac OS X and run it using DNX.
To run this demo you'll need an Apple Mac running OS X. In addition, you
can use Parallels to create a virtual machine running OS X. Detailed instructions
for the demo can be found here:
http://blog.tonysneed.com/2015/05/25/develop-and-deploy-asp-net-5-apps-on-mac-os-x

The following steps begin after Homebrew has been installed:

1. Install DNX using Homebrew
   - Enter the following commands at a Terminal window:
   
   brew tap aspnet/dnx
   brew install dnvm
   source dnvm.sh
   
2. Bring up the DNX version manager, then list installed DNX runtimes.

   dnvm
   dnvm list
   
   - You should see the following output:
   
  Active Version              Runtime Arch Location             Alias
  ------ -------              ------- ---- --------             -----
    *    1.0.0-beta4          mono         ~/.dnx/runtimes      default   
   

3. Change to the ConsoleApp directory
   - Extracted files location for: http://bit.ly/webapi-vnext-webinar
   - Demos/1-Cross-Plat-Hello-World/ConsoleApp
   
4. Restore dependencies

   dnu restore
   
5. Run the console app
  
   dnx . run
   
   - You should need the following output:
   
   Hello World from Cross-Platform ASP.NET 5!
   
