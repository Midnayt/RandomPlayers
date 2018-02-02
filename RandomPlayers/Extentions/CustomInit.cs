using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RandomPlayers.Services;

namespace RandomPlayers.Extentions {
    public class CustomInit : Init {

        //protected override MVVMForms.Navigations.INavigation GetNavigationInstance() {
        //    return new CustomNavigation();
        //}

        //protected override System.Collections.Generic.List<object> GetListInstances() {
        //    var instances = base.GetListInstances();
        //    instances.Add(DependencyService.Get<INativeReflection>());
        //    instances.Add(DependencyService.Get<IFileHelper>());
        //    instances.Add(DependencyService.Get<ITransformationGenerator>());
        //    instances.Add(DependencyService.Get<IMediaPickerService>());
        //    instances.Add(DependencyService.Get<INativeThread>());
        //    instances.Add(DependencyService.Get<INative>());
        //    instances.Add(DependencyService.Get<ISocial>());
        //    return instances;
        //}

        protected override void BuildContainer() {
            base.BuildContainer();
            AddInstance(new ApiService());
            AddInstance(new LocalProviderService());

            //         AddInstance(new LocalProviderService());
            //AddInstance(new ApiService());
        }
    }
}