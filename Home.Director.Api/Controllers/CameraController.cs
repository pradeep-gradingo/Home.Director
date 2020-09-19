using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Config;
using MMALSharp.Handlers;
using MMALSharp.Native;

namespace Home.Director.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CameraController : ControllerBase
    {
        private readonly ILogger<CameraController> _logger;
        private readonly IOptionsSnapshot<Settings> _optionsSettings;
        private readonly MMALCamera _camera;

        public CameraController(ILogger<CameraController> logger, IOptionsSnapshot<Settings> optionsSettings)
        {
            _logger = logger;
            _optionsSettings = optionsSettings;

            //configure
            MMALCameraConfig.Annotate = new AnnotateImage("Home Living Room", 18, System.Drawing.Color.Green)
            {
                ShowDateText = true,
                ShowTimeText = true,
                XOffset = 100,
                YOffset = 100
            };
            MMALCameraConfig.Annotate.ShowDateText = true;
            MMALCameraConfig.Annotate.ShowTimeText = true;
            MMAL_PARAM_MIRROR_T mirror = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_NONE;
            if (_optionsSettings.Value.FlipHorizontal && _optionsSettings.Value.FlipVertical)
                mirror = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_BOTH;
            else if (_optionsSettings.Value.FlipVertical)
                mirror = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_VERTICAL;
            else if (_optionsSettings.Value.FlipHorizontal)
                mirror = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_HORIZONTAL;
            MMALCameraConfig.Flips = mirror;
            MMALCameraConfig.StillResolution = Resolution.As1080p;
            _camera = MMALCamera.Instance;
        }

        [HttpGet]
        [Route("images")]
        public async Task<ActionResult> GetImage()
        {
            if (DateTime.Now.Hour > 18 || DateTime.Now.Hour < 7)
            {
                _logger.LogInformation("Tuning for night mode...");
                MMALCameraConfig.ExposureCompensation = (int)MMAL_PARAM_EXPOSUREMODE_T.MMAL_PARAM_EXPOSUREMODE_NIGHT;
            }
            else
            {
                _logger.LogInformation("Tuning for normal mode...");
                MMALCameraConfig.ExposureCompensation = (int)MMAL_PARAM_EXPOSUREMODE_T.MMAL_PARAM_EXPOSUREMODE_OFF;
            }
            byte[] imageData;
            using (var imgCaptureHandler = new MemoryStreamCaptureHandler())
            {
                _logger.LogInformation("Capturing the picture...");
                await _camera.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
                imageData = new byte[imgCaptureHandler.CurrentStream.Length];
                imgCaptureHandler.CurrentStream.Seek(0, System.IO.SeekOrigin.Begin);
                imgCaptureHandler.CurrentStream.Read(imageData, 0, imageData.Length);
                _logger.LogInformation("Captured the picture...");
            }
            return File(imageData, "image/jpeg");
        }
    }
}
