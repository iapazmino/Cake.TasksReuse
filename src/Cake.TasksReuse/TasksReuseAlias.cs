using System;
using System.IO;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Newtonsoft.Json;

namespace Cake.TasksReuse
{
    [CakeAliasCategory("TasksReuse")]
    public static class TasksReuseAlias
    {
        [CakeMethodAlias]
        public static BuildRunContext ParseContext(this ICakeContext cakeContext, string jsonFilePath)
        {
            if(string.IsNullOrEmpty(jsonFilePath))
            {
                throw new ArgumentException("The path to the JSON config file is missing.");
            }

            var jsonAbsPath = new FilePath(jsonFilePath).MakeAbsolute(cakeContext.Environment).FullPath;
            var json = File.ReadAllText(jsonAbsPath);
            return JsonConvert.DeserializeObject<BuildRunContext>(json);
        }
    }
}
