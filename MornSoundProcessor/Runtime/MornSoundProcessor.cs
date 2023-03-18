using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MornSoundProcessor
{
    public static class MornSoundProcessor
    {
        public static AudioClip NormalizeAmplitude(AudioClip clip, float maxAmplitude)
        {
            var samples = clip.samples; //サンプル数（波形の個数）
            var frequency = clip.frequency; //周波数（1秒あたりの分割数）
            var channels = clip.channels; //モノラルかステレオか。１か２か。
            var data = new float[samples * channels]; //波形
            clip.GetData(data, 0);
            var max = Mathf.Max(Mathf.Abs(data.Min()), Mathf.Abs(data.Max()));
            var rate = maxAmplitude / max;
            for (var i = 0; i < data.Length; i++)
            {
                data[i] *= rate;
            }

            var normalizeClip = AudioClip.Create(clip.name, samples, channels, frequency, clip.loadType == AudioClipLoadType.Streaming);
            normalizeClip.SetData(data, 0);
            return normalizeClip;
        }

        public static AudioClip CutBeginningSilence(AudioClip clip, int beginOffsetSample, float beginAmplitude)
        {
            var samples = clip.samples; //サンプル数（波形の個数）
            var frequency = clip.frequency; //周波数（1秒あたりの分割数）
            var channels = clip.channels; //モノラルかステレオか。１か２か。
            var data = new float[samples * channels]; //波形
            clip.GetData(data, 0);
            var startIndex = GetSoundBeginningIndex(data, beginAmplitude, channels);
            startIndex = Mathf.Max(startIndex - beginOffsetSample * channels, 0);
            var newSamples = samples - startIndex / channels;
            var cutClip = AudioClip.Create(clip.name, newSamples, channels, frequency, clip.loadType == AudioClipLoadType.Streaming);
            var cachedArray = new float[newSamples * channels];
            for (var i = 0; i < newSamples * channels; i++)
            {
                cachedArray[i] = data[startIndex + i];
            }

            cutClip.SetData(cachedArray, 0);
            return cutClip;
        }

        public static AudioClip CutEndingSilence(AudioClip clip, int endOffsetSample, float endAmplitude)
        {
            var samples = clip.samples; //サンプル数（波形の個数）
            var frequency = clip.frequency; //周波数（1秒あたりの分割数）
            var channels = clip.channels; //モノラルかステレオか。１か２か。
            var data = new float[samples * channels]; //波形
            clip.GetData(data, 0);
            var endIndex = GetSoundEndingIndex(data, endAmplitude, channels);
            endIndex = Mathf.Min(endIndex + endOffsetSample * channels, samples * channels);
            var newSamples = endIndex / channels;
            var cutClip = AudioClip.Create(clip.name, newSamples, channels, frequency, clip.loadType == AudioClipLoadType.Streaming);
            var cachedArray = new float[newSamples * channels];
            for (var i = 0; i < newSamples * channels; i++)
            {
                cachedArray[i] = data[i];
            }

            cutClip.SetData(cachedArray, 0);
            return cutClip;
        }

        private static int GetSoundBeginningIndex(IReadOnlyList<float> list, float minAmplitude, int channels)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (Mathf.Abs(list[i]) > minAmplitude)
                {
                    return i - i % channels;
                }
            }

            return 0;
        }

        private static int GetSoundEndingIndex(IReadOnlyList<float> list, float minAmplitude, int channels)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (Mathf.Abs(list[i]) > minAmplitude)
                {
                    return i - i % channels;
                }
            }

            return list.Count - 1 - (list.Count - 1) % channels;
        }

        public static void SaveAudioClipToWav(AudioClip clip, string path)
        {
            var samples = clip.samples; //サンプル数（波形の個数）
            var frequency = clip.frequency; //周波数（1秒あたりの分割数）
            var channels = clip.channels; //モノラルかステレオか。１か２か。
            var data = new float[samples * channels]; //波形
            clip.GetData(data, 0);

            // 1~4バイト
            // 右チャンネル2バイト 左チャンネル2バイト
            //16bit：符号ビットあり 補数表現
            //2の補数表現 反転して+1
            //AudioClip流 -1.0 ~ 1.0
            //総データ数 ... 4byte * samples + 固定
            using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            WriteWavHeader(fileStream, samples, (short)channels, frequency);
            foreach (var value in data)
            {
                //符号bitあり
                WriteShortLittleEndian(fileStream, value > 0 ? (short)(short.MaxValue * value) : (short)(short.MinValue * -value));
            }
        }

        private static void WriteWavHeader(Stream stream, int samples, short channels, int frequency)
        {
            var fileSize = 2 * samples * channels + 44;
            stream.Seek(0, SeekOrigin.Begin);

            //RIFF識別子 4byte
            stream.WriteByte((byte)'R');
            stream.WriteByte((byte)'I');
            stream.WriteByte((byte)'F');
            stream.WriteByte((byte)'F');

            //チャンクサイズ 4byte
            WriteIntLittleEndian(stream, fileSize - 8);

            //フォーマット 4byte
            stream.WriteByte((byte)'W');
            stream.WriteByte((byte)'A');
            stream.WriteByte((byte)'V');
            stream.WriteByte((byte)'E');

            //fmt識別子 4byte
            stream.WriteByte((byte)'f');
            stream.WriteByte((byte)'m');
            stream.WriteByte((byte)'t');
            stream.WriteByte((byte)' ');

            //fmtチャンクのバイト数 4byte
            WriteIntLittleEndian(stream, 16);

            //音声フォーマット 2byte
            WriteShortLittleEndian(stream, 1);

            //チャンネル数 2byte
            WriteShortLittleEndian(stream, channels);

            //サンプリング周波数 4byte
            WriteIntLittleEndian(stream, frequency);

            //1秒あたりバイト数の平均 4byte （サンプリング周波数*ブロックサイズ）
            WriteIntLittleEndian(stream, frequency * 2 * channels);

            //ブロックサイズ 2byte （チャンネル数*1サンプルあたりのビット数/8）
            WriteShortLittleEndian(stream, (short)(2 * channels));

            //ビット/サンプル 2byte
            WriteShortLittleEndian(stream, 16);

            //サブチャンク識別子 4byte
            stream.WriteByte((byte)'d');
            stream.WriteByte((byte)'a');
            stream.WriteByte((byte)'t');
            stream.WriteByte((byte)'a');

            //サブチャンクサイズ 4byte
            WriteIntLittleEndian(stream, 2 * samples * channels);
        }

        private static void WriteIntLittleEndian(Stream stream, int value)
        {
            stream.WriteByte((byte)value); // 8~1bit
            stream.WriteByte((byte)(value >> 8)); //16~9bit
            stream.WriteByte((byte)(value >> 16)); //24~17bit
            stream.WriteByte((byte)(value >> 24)); //32~25bit
        }

        private static void WriteShortLittleEndian(Stream stream, short value)
        {
            stream.WriteByte((byte)value); // 8~1bit
            stream.WriteByte((byte)(value >> 8)); //16~9bit
        }
    }
}
