using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMBEditor
{
    /// <summary>
    /// Permite que implementacoes sejam vistas pelo menu de apresentecao do <see cref="GMBWindow"/>
    /// </summary>
    public interface IGMBEditorWindow
    {
        /// <summary>
        /// Informa o nome do VisualElement utilizado como botao de menu pela janela principal <see cref="GMBWindow"/>
        /// </summary>
        /// <returns></returns>
        GMBWindowMenuItem GetGMBWindowMenuItem();
        GMBWindow GetGMBWindow();
        void CreateGUI(GMBWindow gmbWindow);

        void CloseGUI();
    }
}
