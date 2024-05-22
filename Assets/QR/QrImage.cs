using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityToolkit.UI.QR
{
    [RequireComponent(typeof(RawImage))]
    public class QrImage : MonoBehaviour
    {
        [SerializeField] private string _url;

        private RawImage _image;
        private QrCodeGenerator _generator;

        private void Awake()
        {
            _image = GetComponent<RawImage>();
            _generator = new QrCodeGenerator();
        }

        private void Start()
        {
            UpdateTexture(_url);
        }

        private void UpdateTexture(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            var data = _generator.CreateQrCode(url, QrCodeGenerator.ECCLevel.Q);
            var qrCode = new QrCode(data);
            _image.material.mainTexture = qrCode.GetGraphic(20);
        }

        public void SetUrl(string url)
        {
            _url = url;
            UpdateTexture(_url);
        }
    }
}