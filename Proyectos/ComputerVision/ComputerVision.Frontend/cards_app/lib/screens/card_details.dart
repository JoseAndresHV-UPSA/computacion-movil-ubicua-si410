import 'package:cards_app/models/image_info.dart' as model;
import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import '/widgets/choice_chip.dart';
import '/widgets/multiple_choice_chip.dart';

class CardDetailsScreen extends StatefulWidget {
  final model.ImageInfo? imageInfo;
  const CardDetailsScreen({Key? key, required this.imageInfo})
      : super(key: key);

  @override
  State<CardDetailsScreen> createState() => _CardDetailsScreenState();
}

class _CardDetailsScreenState extends State<CardDetailsScreen> {
  List<String> selectedOptions = [];

  Future<void> _launch(Uri url) async {
    if (!await launchUrl(
      url,
      mode: LaunchMode.externalApplication,
    )) {
      throw 'Could not launch $url';
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            title: Container(
          padding: const EdgeInsets.only(left: 82),
          child: const Text('Cards App'),
        )),
        body: SafeArea(
            child: Center(
          child: ListView(padding: const EdgeInsets.all(6.0), children: [
            const Text.rich(TextSpan(
              children: [
                WidgetSpan(child: Icon(Icons.info_outline)),
                TextSpan(
                  text: " Text",
                  style: TextStyle(fontSize: 22.0, fontWeight: FontWeight.bold),
                )
              ],
            )),
            MultipleChoiceChipWidget(
              options: widget.imageInfo!.text,
              selectedOptions: selectedOptions,
            ),
            const Divider(),
            const Text.rich(TextSpan(
              children: [
                WidgetSpan(child: Icon(Icons.email_outlined)),
                TextSpan(
                  text: " Emails",
                  style: TextStyle(fontSize: 22.0, fontWeight: FontWeight.bold),
                )
              ],
            )),
            ChoiceChipWidget(options: widget.imageInfo!.emails, type: 3),
            const Divider(),
            const Text.rich(TextSpan(
              children: [
                WidgetSpan(child: Icon(Icons.link)),
                TextSpan(
                  text: " URLs",
                  style: TextStyle(fontSize: 22.0, fontWeight: FontWeight.bold),
                )
              ],
            )),
            ChoiceChipWidget(options: widget.imageInfo!.urls, type: 1),
            const Divider(),
            const Text.rich(TextSpan(
              children: [
                WidgetSpan(child: Icon(Icons.phone)),
                TextSpan(
                  text: " Phone Numbers",
                  style: TextStyle(fontSize: 22.0, fontWeight: FontWeight.bold),
                )
              ],
            )),
            ChoiceChipWidget(options: widget.imageInfo!.phoneNumbers, type: 2),
          ]),
        )),
        floatingActionButton: FloatingActionButton(
            onPressed: () {
              String search = "";
              for (var element in selectedOptions) {
                search += element + " ";
              }
              _launch(Uri(
                  scheme: 'https',
                  host: 'www.google.com',
                  path: '/search',
                  queryParameters: {'q': search}));
            },
            child: const Icon(Icons.search)));
  }
}
