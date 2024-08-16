using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System.Linq;
using UnityEditor;

#if UNITY_EDITOR

using System.Reflection;
using UnityEditor.Experimental.GraphView;
#endif


namespace GMB
{

    /// <summary>
    /// Garante que as implementacoes tenham a extrutura basica de dados obrigatorios de GMB
    /// - ID Automatico, Data de criacao e Modificacao. 
    /// Utilize para implementar todos os tipos de dados GMB. 
    /// </summary>
    [ResourcesPath("")]
    public abstract class Data : ScriptableObject
    {
        [SerializeField, ReadOnly] private int _aid = -1;
        [SerializeField, ReadOnly] private string _id = Guid.NewGuid().ToString().ToUpper();
        [SerializeField, ReadOnly] private string _createdDate = DateTime.Now.ToString(StringsProvider._DATE_TIME_FORMAT_);
        [SerializeField] private string _friendlyName = StringsProvider._UNDEFINED_;
        [SerializeField] private string _description = "";
        [SerializeField] private Sprite _icon = null;

        /// <summary>
        /// Auto incremented ID
        /// <para>
        /// - ID numerico Único e auto incrementado. Setado quando este dado foi criado pelo GMB Editor. Utilize este ID para salvar e ordenar os dados de maneira consistente e persistente
        /// em instancias de inicializacao e banco dados.
        /// </para>
        /// A unica maneira de haver discrepancia com dados perssistentes e se o ID de todos os arquivos previamente cadastrados forem (Resetados). Isto
        /// atualmente e possivel, atravez de configuracoes do GMB
        /// <para>
        /// Importante: Este ID ainda esta em avalicao. Ele é automaticamente incrimentado apartir do GMBSettings, scriptable de configuracoes do GMBEditor. 
        /// </para>
        /// </summary>
        /// <returns></returns>
        public int GetAID()
        {
            return _aid;
        }

        /// <summary>
        /// Tick Time ID: ID Baseado no numero de ticks da data de criacao do dado. Pode ser utilizado como ID Unico e auto incrementado, para para
        /// ordenacao de arrays consistente, bem como <see cref="GetAID"/>
        /// </summary>
        /// <returns></returns>
        public long GetTID()
        {
            return DateTime.Parse(_createdDate).Ticks;
        }


        /// <summary>
        /// ID Unico deste Dado. Util para garantir persistencia de dados. Voce tambem pode querer utilizar
        /// <see cref="GetAID"/>, para uma versao mais eficiente de persistencia.
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return _id;
        }
        public string GetCreatedDate()
        {
            return _createdDate;
        }

        /// <summary>
        /// Nome cadastrado para este dado, pode haver nomes repetidos
        /// </summary>
        /// <returns></returns>
        public virtual string GetFriendlyName()
        {
            return _friendlyName;
        }

        public virtual string GetDescription()
        {
            return _description;
        }
        public virtual Sprite GetIcon()
        {

            return _icon;
        }

        /// <summary>
        /// Retorna o nome do arquivo como um caminho relativo para ser usado por provedoers de pesquisa.
        /// Se este dado contem categoria, ele devera retornar a categoria + o nome. Ex:
        /// categoriaName/dataName
        /// </summary>
        /// <returns></returns>
        public virtual string GetNameAsRelativePath()
        {
            return _friendlyName;
        }


#if UNITY_EDITOR

        public UnityEditor.SerializedObject GetSerializedObject()
        {
            return new UnityEditor.SerializedObject(this);
        }
        // <summary>
        ///   <para>Returns the path name relative to the project folder where the asset is stored.</para>
        /// </summary>
        /// <returns>
        ///   <para>The asset path name, or null, or an empty string if the asset does not exist.</para>
        /// </returns>
        public string GetAssetPath()
        {
            return AssetDatabase.GetAssetPath(this);
        }

        public void SaveAsset()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

#endif
    }

}