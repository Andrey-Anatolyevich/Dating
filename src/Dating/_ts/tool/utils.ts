class Utils {
    public static decodeHtml(html: string): string {
        var txt = document.createElement("textarea");
        txt.innerHTML = html;
        return txt.value;
    }
}