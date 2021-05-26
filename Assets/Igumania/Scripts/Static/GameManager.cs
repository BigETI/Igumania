namespace Igumania
{
    public static class GameManager
    {
        private static byte selectedProfileIndex;

        private static IProfile selectedProfile;

        public static byte SelectedProfileIndex
        {
            get => selectedProfileIndex;
            set
            {
                if (selectedProfileIndex != value)
                {
                    selectedProfileIndex = value;
                    selectedProfile = Profiles.LoadProfile(selectedProfileIndex);
                }
            }
        }

        public static IProfile SelectedProfile => selectedProfile ??= Profiles.LoadProfile(selectedProfileIndex);

        public static void ReloadSelectedProfile() => selectedProfile = Profiles.LoadProfile(selectedProfileIndex);
    }
}
