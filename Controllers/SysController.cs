using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace fxmgmt.Controllers
{
    [ApiController]
    public class SysController : ControllerBase
    {
        public SysController(IFeatureManager featureManager)
        {
            FeatureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        public IFeatureManager FeatureManager { get; }

        [HttpGet("sys")]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            if (await FeatureManager.IsEnabledAsync("SysInfo").ConfigureAwait(false))
            {
                var info = new
                {
                    ProcessId = Process.GetCurrentProcess().Id,
                    UpTimeMs = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds,
                    Host = Environment.MachineName,
                    ServiceAccount = Environment.UserName
                };
                return Ok(info);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
