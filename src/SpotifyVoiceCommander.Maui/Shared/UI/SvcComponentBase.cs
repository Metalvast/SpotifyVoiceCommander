namespace SpotifyVoiceCommander.Maui.Shared.UI;

public abstract class SvcComponentBase : EtaiComponentBase
{
    #region UI Fields

    private TagId c_tagId;
    public TagId TagId => c_tagId;

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        base.OnInitialized();

        c_tagId = new(RootClassName);
    }

    #endregion
}
