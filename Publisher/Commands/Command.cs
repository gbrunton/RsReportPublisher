using FubuCore.CommandLine;
using StructureMap;
using StructureMap.Graph;

namespace Publisher.Commands
{
	public abstract class Command<TInputModel> : FubuCommand<TInputModel>, IFubuCommand
	{
		public override bool Execute(TInputModel inputModel)
		{
			ObjectFactory.Configure(config =>
			{
				config.Scan(scanner =>
				{
					scanner.TheCallingAssembly();
					scanner.WithDefaultConventions();
					scanner.RegisterConcreteTypesAgainstTheFirstInterface();
					this.ManageScanner(scanner);
				});
				this.Config(config, inputModel);
			});

			return ((Command<TInputModel>)ObjectFactory.GetInstance(this.GetType())).ExecuteHydratedObject(inputModel);
		}

		protected virtual void Config(ConfigurationExpression config, TInputModel inputModel) { }
		protected virtual void ManageScanner(IAssemblyScanner scanner) { }
		protected abstract bool ExecuteHydratedObject(TInputModel inputModel);
	}
}