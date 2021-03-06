﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace TartuNLP
{
    [TranslationProviderWinFormsUi(Id = "TartuNLP_Translation_Provider_WinFormsUI",
                                   Name = "TartuNLP_Translation_Provider_WinFormsUI",
                                   Description = "TartuNLP Translation Provider WinFormsUI")]
    class TartuNLPProviderWinFormsUI : ITranslationProviderWinFormsUI
    {
        #region ITranslationProviderWinFormsUI Members

        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            if (languagePairs.Length > 0)
            {
                TartuNLPConfigForm dialog = new TartuNLPConfigForm(new TartuNLPOptions(), languagePairs);
                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    TartuNLPProvider testProvider = new TartuNLPProvider(dialog.Options);
                    return new ITranslationProvider[] { testProvider };
                }
            }
            else {
                MessageBox.Show("Please configure at least one language pair before setting up the plugin","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            TartuNLPProvider editProvider = translationProvider as TartuNLPProvider;
            if (editProvider == null)
            {
                return false;
            }

            TartuNLPConfigForm dialog = new TartuNLPConfigForm(editProvider.Options, languagePairs);
            if (dialog.ShowDialog(owner) == DialogResult.OK)
            {
                editProvider.Options = dialog.Options;
                return true;
            }
            return false;
        }

        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            return true;
        }

        public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
        {
            TranslationProviderDisplayInfo info = new TranslationProviderDisplayInfo();
            info.Name = PluginResources.Plugin_NiceName;
            info.TranslationProviderIcon = PluginResources.icon;
            info.SearchResultImage = PluginResources.symbol;

            return info;
        }

        public bool SupportsEditing
        {
            get { return true; }
        }

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
            {
                throw new ArgumentNullException("URI not supported by the plug-in.");
            }
            return String.Equals(translationProviderUri.Scheme, TartuNLPProvider.ListTranslationProviderScheme, StringComparison.CurrentCultureIgnoreCase);
        }

        public string TypeDescription
        {
            get { return PluginResources.Plugin_Description; }
        }

        public string TypeName
        {
            get { return PluginResources.Plugin_NiceName; }
        }

        #endregion
    }
}
