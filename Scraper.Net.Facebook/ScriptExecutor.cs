using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper.Net.Facebook
{
    public static class ScriptExecutor
    {
        public static async Task<string> Execute(
            string command,
            string fileName,
            CancellationToken token = default,
            params object[] parameters)
        {
            var arguments = new[] { fileName }
                .Concat(
                    parameters
                        .Where(o => o != null)
                        .Select(o => o.ToString()));
            
            ProcessStartInfo startInfo = CreateProcessStartInfo(
                command,
                arguments);
            
            using Process process = Process.Start(startInfo);

            token.Register(() => process.Kill());

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync(token);
            
            if (string.IsNullOrEmpty(output))
            {
                throw new InvalidOperationException($"Failed to execute script (no output) {error}");
            }

            return output;
        }

        private static ProcessStartInfo CreateProcessStartInfo(
            string command,
            IEnumerable<string> args)
        {
            return new()
            {
                FileName = command,
                Arguments = string.Join(' ', args),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }
    }
}