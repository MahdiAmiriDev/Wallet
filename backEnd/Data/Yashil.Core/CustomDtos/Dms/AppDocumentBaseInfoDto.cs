using JetBrains.Annotations;

namespace Yashil.Core.CustomDtos.Dms
{
        /// <summary>
        /// اصلاعات پایه سند
        /// </summary>
        public class AppDocumentBaseInfoDto
        {
            /// <summary>
            /// شناسه سند
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// عنوان سند
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// فایل سند
            /// </summary>
            [CanBeNull]
            public byte[] DocumentFile { get; set; }
        }
}
