import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';

class ChoiceChipWidget extends StatefulWidget {
  final List<String>? options;
  final int type;
  const ChoiceChipWidget({Key? key, required this.options, required this.type})
      : super(key: key);

  @override
  State<ChoiceChipWidget> createState() => _ChoiceChipWidgetState();
}

class _ChoiceChipWidgetState extends State<ChoiceChipWidget> {
  Future<void> _launch(Uri url) async {
    if (!await launchUrl(
      url,
      mode: LaunchMode.externalApplication,
    )) {
      throw 'Could not launch $url';
    }
  }

  _buildChoiceList() {
    List<Widget> choices = [];
    for (var option in widget.options!) {
      choices.add(Container(
        padding: const EdgeInsets.symmetric(vertical: 0.0, horizontal: 4.0),
        child: ActionChip(
          label: Text(option),
          labelStyle: const TextStyle(
              color: Colors.black, fontSize: 14.0, fontWeight: FontWeight.bold),
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(30.0)),
          backgroundColor: Colors.grey[200],
          onPressed: () => {
            if (widget.type == 1)
              {_launch(Uri(scheme: 'https', host: option, path: '/'))}
            else if (widget.type == 2)
              {_launch(Uri(scheme: 'tel', host: option))}
            else if (widget.type == 3)
              {_launch(Uri(scheme: 'mailto', path: option))}
          },
        ),
      ));
    }
    return choices;
  }

  @override
  Widget build(BuildContext context) {
    return Wrap(
      children: _buildChoiceList(),
    );
  }
}
