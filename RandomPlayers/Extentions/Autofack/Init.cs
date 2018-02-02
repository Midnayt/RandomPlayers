using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;

namespace RandomPlayers.Extentions {
    public class Init {

        public static Init Instance { get; private set; }

        public static void Initialize(Init init = null) {
            if (init == null)
                init = new Init();

            Instance = init;
            init.BuildContainer();
        }

        protected virtual List<object> GetListInstances() {
            return new List<object> {
                GetNavigationInstance()
            };
        }

        #region containers

        public IContainer Container { get; private set; }

        protected virtual void BuildContainer() {
            var builder = new ContainerBuilder();
            foreach (var instance in GetListInstances()) {
                builder.RegisterInstance(instance).AsImplementedInterfaces();
            }
            Container = builder.Build();
        }

        public static T GetService<T>() {
            if (Instance?.Container == null) {
                var ex = new InitializeExceptions();
                Methods.Report($"MVVMInit.GetService<{typeof(T)?.FullName}>", ex);
                throw ex;
            }
            object result = null;
            if (Instance?.Container.TryResolve(typeof(T), out result) == true)
                return (T)result;
            else
                return default(T);
        }

        public static void AddInstance(object _class) {
            if (Instance?.Container == null) {
                var ex = new InitializeExceptions();
                Methods.Report($"MVVMInit.RegisterServiceForClass>", ex);
                throw ex;
            }
            //Instance.builder.RegisterIn
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_class).AsImplementedInterfaces();
            builder.Update(Instance.Container);
        }

        #endregion

        public Init() { }

        #region instances

        protected virtual Navigation GetNavigationInstance() {
            return new Navigation();
        }

        #endregion
    }
}