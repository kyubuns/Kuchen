namespace Tonari.Text
{ 
    public class MiniRegex
    {
        private const char any = '?';
        private const char wild = '*';
        private const string wildString = "*";

        public static bool IsMatch(string input, string pattern)
        {
            // 同一文字列ならtrue
            if (pattern == input) return true;
            // フォーマットがなければアウト
            if (pattern == string.Empty) return false;
            // テキストが空の時はワイルドだけ認める
            if (input == string.Empty) return IsTailAllWild(pattern, 0);
            // フォーマットがワイルドならガバガバ
            if (pattern == wildString) return true;

            var formatIndex = 0;
            var textIndex = 0;
            while (formatIndex < pattern.Length)
            {
                // 同じ文字 or 単一文字
                if (pattern[formatIndex] == input[textIndex]
                    || pattern[formatIndex] == any)
                {
                    ++formatIndex;
                    ++textIndex;

                    if (textIndex == input.Length)
                    {
                        return IsTailAllWild(pattern, formatIndex);
                    }

                    continue;
                }

                // ワイルドカード
                if (pattern[formatIndex] == wild)
                {
                    // 末尾までワイルドだけならtrue
                    if (IsTailAllWild(pattern, formatIndex)) return true;

                    // ワイルド以外に語尾がある時
                    if (formatIndex < pattern.Length - 1)
                    {
                        // 移動させたテキスト量
                        var shifted = 0;

                        while (textIndex + shifted < input.Length)
                        {
                            // ワイルドの次の文字と一致する文字がある（or続いている）ときの処理
                            if (pattern[formatIndex + 1] == input[textIndex + shifted]
                                || pattern[formatIndex + 1] == any)
                            {
                                ++shifted;

                                // 最後まで同じ文字じゃったな…
                                if (textIndex + shifted == input.Length)
                                    return true;

                                // 違う文字がきたらbreakなのじゃ
                                if (pattern[formatIndex + 1] != input[textIndex + shifted]
                                    && pattern[formatIndex + 1] != any)
                                {
                                    ++formatIndex;
                                    textIndex += shifted - 1;
                                    break;
                                }

                                // 同じ文字がまだまだ続く
                                if (textIndex + shifted < input.Length)
                                    continue;
                            }

                            // ワイルドの次の文字と違う文字の場合は素直にその次へ
                            ++shifted;

                            // 最後まで違う文字じゃったな…
                            if (textIndex + shifted == input.Length)
                                return false;
                        }

                        continue;
                    }
                }

                return false;
            }

            return false;
        }

        private static bool IsTailAllWild(string format, int startIndex)
        {
            for (var i = startIndex; i < format.Length; ++i)
            {
                if (format[i] != wild)
                    return false;
            }
            return true;
        }
    }
}