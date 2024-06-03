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

        private void Awake()
        {
            _qrImage = GetComponent<QrImage>();
        }

        private void OnEnable()
        {
            _scapeXMobile.OnConfigUpdate += UpdateCode;
        }

        private void OnDisable()
        {
            _scapeXMobile.OnConfigUpdate -= UpdateCode;
        }

        private void UpdateCode(SxmConfig config)
        {
            if (_baseUrl == string.Empty)
            {
                _baseUrl = _qrImage.Url;
            }

            _qrImage.Url = $"{_baseUrl}?r={config.RoomId}";
            _qrImage.Url += string.IsNullOrEmpty(config.MqttUrl) ? string.Empty : $"&u={config.MqttUrl}";
        }
    }
}
