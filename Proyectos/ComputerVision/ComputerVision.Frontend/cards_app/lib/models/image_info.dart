class ImageInfo {
  final List<String>? text;
  final List<String>? emails;
  final List<String>? urls;
  final List<String>? phoneNumbers;

  ImageInfo({
    required this.text,
    required this.emails,
    required this.urls,
    required this.phoneNumbers,
  });

  factory ImageInfo.fromJson(Map<String, dynamic> json) {
    return ImageInfo(
      text: List<String>.from((json['text'] ?? []) as List),
      emails: List<String>.from((json['emails'] ?? []) as List),
      urls: List<String>.from((json['urls'] ?? []) as List),
      phoneNumbers: List<String>.from((json['phoneNumbers'] ?? []) as List),
    );
  }
}
