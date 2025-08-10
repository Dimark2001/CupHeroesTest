public class HeroView : BaseCharacterView
{
    public override CharacterModel CharacterModel
    {
        get
        {
            if (_model == null)
            {
                CharacterModel = GameResources.HeroConfig.GetModel();
            }

            return _model;
        }
        protected set => _model = value;
    }

    private CharacterModel _model;
}