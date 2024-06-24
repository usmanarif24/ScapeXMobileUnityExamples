using SxmExamples.QR;
using TuioUnity.Tuio20.Sxm;
using UnityEngine;

namespace SxmExamples
{
    [RequireComponent(typeof(QrImage))]
    public class QrUpdater : MonoBehaviour
    {
        [SerializeField] private ScapeXMobile _scapeXMobile;

        private QrImage _qrImage;
        private string _baseUrl = string.Empty;

        private string _webAppUrl;

        private void Awake()
        {
            _qrImage = GetComponent<QrImage>();
        }

        private void OnEnable()
        {
            _scapeXMobile.OnConfigUpdate += UpdateQrCode;
        }

        private void OnDisable()
        {
            _scapeXMobile.OnConfigUpdate -= UpdateQrCode;
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(_webAppUrl) || _qrImage.Url == _webAppUrl) return;
            _qrImage.Url = _webAppUrl;
        }
        
        private void UpdateQrCode(object sender, SxmEventArgs sxmConfig)
        {
            if (_baseUrl == string.Empty)
            {
                _baseUrl = _qrImage.Url;
            }
            _webAppUrl = $"{_baseUrl}?r={sxmConfig.RoomId}";
            _webAppUrl += string.IsNullOrEmpty(sxmConfig.MqttUrl) ? string.Empty : $"&u={sxmConfig.MqttUrl}";
        }
    }
}
