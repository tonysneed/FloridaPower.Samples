Demo: ASP.NET 5 Cross-Platform Hello Web

In this demo we'll deploy a web app to Linux and run it using DNX.
To run this demo you'll need a virtual machine running Linux Ubuntu
v 14.04 or greater. Detailed instructions for the demo can be found here:
http://blog.tonysneed.com/2015/05/25/develop-and-deploy-asp-net-5-apps-on-linux

The following steps begin after Mono and libuv been installed:

1. Install DNX using Curl
   - Enter the following commands at a Terminal window:
   
   curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.s
   source ~/.dnx/dnvm/dnvm.sh
   
2. Bring up the DNX version manager, then install the latest DNX runtime.

   dnvm
   dnvm upgrade
   
3. List installed DNX runtimes.

   dnvm list

   - You should see the following output:
   
  Active Version              Runtime Arch Location             Alias
  ------ -------              ------- ---- --------             -----
    *    1.0.0-beta4          mono         ~/.dnx/runtimes      default   
   
4. Change to the WebApp directory
   - Extracted files location for: http://bit.ly/webapi-vnext-webinar
   - Demos/2-Cross-Plat-Hello-WebAPI/Before/WebApp
   
5. Restore dependencies

   dnu restore
   
6. Run the web app
  
   dnx . kestrel
   
7. Open Firefox and navigate to: http://localhost:5004/
   - You should see the ASP.NET 5 welcome page