using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSeeders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForClient",
                table: "TermsAndCondetions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Type" },
                values: new object[] { "اشعار جديد", "Other" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Type" },
                values: new object[] { "الطلب مقبول", "Acceptance" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم تأكيد الطلب", "Confirmation" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم تعيين العامل", "Assignment" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Type" },
                values: new object[] { "انتهاء الطلب", "Completion" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Type" },
                values: new object[] { "الطلب ملغي", "Cancellation" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ForClient", "Title", "Type" },
                values: new object[] { true, "عامل ألغى الموعد", "Cancellation" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Title", "Type" },
                values: new object[] { "العرض مقبول", "Acceptance" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم تعديل الرصيد", "Confirmation" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Title", "Type" },
                values: new object[] { "طلب خدمة جديد", "Assignment" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Title", "Type" },
                values: new object[] { "انتهاء الطلب", "Completion" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Title", "Type" },
                values: new object[] { "العرض مرفوض", "Cancellation" });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "CreatedAt", "ForClient", "ModifiedAt", "Title", "Type" },
                values: new object[] { 13, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null, "الطلب ملغي", "Cancellation" });

            migrationBuilder.InsertData(
                table: "SuspendReasons",
                columns: new[] { "Id", "CreatedAt", "ModifiedAt", "Reason" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), null, "مديونية" },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), null, "انتهاك اتفاقية الاستخدام" },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), null, "إلغاء متكرر" },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), null, "سوء معاملة" }
                });

            migrationBuilder.InsertData(
                table: "TermsAndCondetions",
                columns: new[] { "Id", "Content", "CreatedAt", "ForClient", "ModifiedAt", "Title" },
                values: new object[,]
                {
                    { 1, "تعدّ هذه الاتفاقية بمنزلة اتفاقية عامة تحكم ضوابط التعامل بين المستخدم والموقع، وتسري على جميع الخدمات التي يقدمها الموقع للمستخدم.\n\r\nقبول الاتفاقية: يلتزم المستخدم بجميع الشروط الواردة في هذه الاتفاقية لضمان استعمال الخدمة، كما أنّ استمرار استعمال الخدمة يعدّ موافقةً ضمنية من جهة المستخدم على بنود اتفاقية الاستخدام.\r\n\r\nحقوق الملكية: إنّ المحتوى والتنظيم والتصميم والتجميع والترجمة وجميع المواد الأخرى المتعلقة بالمركز محمية بموجب قوانين حقوق المؤلف والعلامات التجارية وحقوق الملكية الأخرى السارية المفعول. ولا يحق للمستخدم نسخ أو نشر أو توزيع أيّ مواد منشورة على المركز ومنسوبة للموقع من دون ذكر مصدرها، ولا يجوز تعديل المادة الموجودة في هذا الموقع أو تحويرها أو اقتباسها لخلق عمل جديد أو استخدامها لأيّ غرض بخلاف الاستعمال الشخصي غير التجاري. ويحتفظ المركز بجميع الحقوق القانونية لمقاضاة من يخالف هذا الشرط، طبقًا لقوانين الملكية الفكرية.\r\n\r\nحق الاستخدام والاقتباس العلمي: إنّ استعراض أو طباعة أو تحميل أي محتوى أو رسم أو نموذج من المركز يخوّل ترخيصًا محدودًا وحصريًا للاستعمال الشخصي والمنصف. ويلتزم المستخدم في حال الاقتباس العلمي و/ أو استخدام أيٍّ من المواد البحثية والدراسات المنشورة في الموقع بالإشارة إلى مصدرها وفقًا للأعراف الأكاديمية المعتمدة.\r\n\r\nالتحرير والحذف والتعديل: يحتفظ المركز بجميع الحقوق في تغيير أو تعديل أو إلغاء أو تبديل كل خدمات المركز أو جزء منها. ويحقّ للمركز عدم نشر و/ أو حذف أيّ مادة أو تعليق أو صورة لا تتوافق مع شروط هذه الاتفاقية أو لا تتناسب مع سياسة المركز. كما يحق للمركز إلغاء التسجيل (إن وُجد). يحتفظ المركز بجميع الحقوق في إلغاء أو إيقاف أيّ حق في استعمال الخدمة في حالة انتهاك المستخدم أيّ بند من بنود اتفاقية الاستخدام.\r\n\r\nعدم القابلية للتحويل: حق استعمال خدمات الموقع وأي كلمة مرور للحصول على المعلومات أو الوثائق غير قابل للتحويل.\r\n\r\nالإقرار بالمسؤولية: يلتزم المستخدم بالحفاظ على سرية بيانات حسابه (إن وجد)، بما في ذلك اسم المستخدم وكلمة السر الخاصة به، كما يعدّ مسؤولًا مسؤولية كاملة عن أيّ استعمال للخدمة يجري من خلال اسم المستخدم وكلمة السر الخاصة به، سواء جرى ذلك من طرفه أو من طرف آخرين.\r\n\r\nسرية البيانات: يحتفظ المركز بحقه في جمع واستخدام معلومات عن المستخدم كالتي يجري تعبئتها في استمارة التسجيل، وذلك للأغراض الإحصائية وتحسين الخدمة.\r\n\r\nخدمات المركز: لا يتحمل المركز أو أي شخص يشترك في إعداد، أو إنتاج، أو توزيع أي مادة في المركز، أية مسؤولية عن أيّ ضرر مباشر أو غير مباشر مادي أو معنوي ينشأ من استعمال هذا الموقع، أو من عدم التمكن من استعماله، أو من أي خطأ أو حذف، أو عيب، يوجد فيه، أو من عدم صحة المعلومات التي يقدمها أو من أي تأخير أو انقطاع في بثّه.\r\n\r\nمعاودة النشر: لا يحقّ للباحث الذي قدّم بحثًا في أحد مؤتمرات المركز، أو نُشر له في إحدى دوريات المركز، أن يعيد نشره إلا بموجب إذنٍ خطّي مُسبق من إدارة المركز. وفي حال الموافقة، يشترط عليه أن يشير إلى أنّ هذا البحث قدم إلى مؤتمر المركز أو إلى الدورية المعنية مع تحديد العدد والتاريخ.\r\n\r\nأحكام عامة: تطبق على هذه الشروط قوانين دولة قطر ويكون لمحاكمها سلطة حصرية في نظر النزاعات الناشئة عنها.\r\n\r\n", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), true, null, "اتفاقية الاستخدام للعملاء" },
                    { 2, "تعدّ هذه الاتفاقية بمنزلة اتفاقية عامة تحكم ضوابط التعامل بين المستخدم والموقع، وتسري على جميع الخدمات التي يقدمها الموقع للمستخدم.\n\r\nقبول الاتفاقية: يلتزم المستخدم بجميع الشروط الواردة في هذه الاتفاقية لضمان استعمال الخدمة، كما أنّ استمرار استعمال الخدمة يعدّ موافقةً ضمنية من جهة المستخدم على بنود اتفاقية الاستخدام.\r\n\r\nحقوق الملكية: إنّ المحتوى والتنظيم والتصميم والتجميع والترجمة وجميع المواد الأخرى المتعلقة بالمركز محمية بموجب قوانين حقوق المؤلف والعلامات التجارية وحقوق الملكية الأخرى السارية المفعول. ولا يحق للمستخدم نسخ أو نشر أو توزيع أيّ مواد منشورة على المركز ومنسوبة للموقع من دون ذكر مصدرها، ولا يجوز تعديل المادة الموجودة في هذا الموقع أو تحويرها أو اقتباسها لخلق عمل جديد أو استخدامها لأيّ غرض بخلاف الاستعمال الشخصي غير التجاري. ويحتفظ المركز بجميع الحقوق القانونية لمقاضاة من يخالف هذا الشرط، طبقًا لقوانين الملكية الفكرية.\r\n\r\nحق الاستخدام والاقتباس العلمي: إنّ استعراض أو طباعة أو تحميل أي محتوى أو رسم أو نموذج من المركز يخوّل ترخيصًا محدودًا وحصريًا للاستعمال الشخصي والمنصف. ويلتزم المستخدم في حال الاقتباس العلمي و/ أو استخدام أيٍّ من المواد البحثية والدراسات المنشورة في الموقع بالإشارة إلى مصدرها وفقًا للأعراف الأكاديمية المعتمدة.\r\n\r\nالتحرير والحذف والتعديل: يحتفظ المركز بجميع الحقوق في تغيير أو تعديل أو إلغاء أو تبديل كل خدمات المركز أو جزء منها. ويحقّ للمركز عدم نشر و/ أو حذف أيّ مادة أو تعليق أو صورة لا تتوافق مع شروط هذه الاتفاقية أو لا تتناسب مع سياسة المركز. كما يحق للمركز إلغاء التسجيل (إن وُجد). يحتفظ المركز بجميع الحقوق في إلغاء أو إيقاف أيّ حق في استعمال الخدمة في حالة انتهاك المستخدم أيّ بند من بنود اتفاقية الاستخدام.\r\n\r\nعدم القابلية للتحويل: حق استعمال خدمات الموقع وأي كلمة مرور للحصول على المعلومات أو الوثائق غير قابل للتحويل.\r\n\r\nالإقرار بالمسؤولية: يلتزم المستخدم بالحفاظ على سرية بيانات حسابه (إن وجد)، بما في ذلك اسم المستخدم وكلمة السر الخاصة به، كما يعدّ مسؤولًا مسؤولية كاملة عن أيّ استعمال للخدمة يجري من خلال اسم المستخدم وكلمة السر الخاصة به، سواء جرى ذلك من طرفه أو من طرف آخرين.\r\n\r\nسرية البيانات: يحتفظ المركز بحقه في جمع واستخدام معلومات عن المستخدم كالتي يجري تعبئتها في استمارة التسجيل، وذلك للأغراض الإحصائية وتحسين الخدمة.\r\n\r\nخدمات المركز: لا يتحمل المركز أو أي شخص يشترك في إعداد، أو إنتاج، أو توزيع أي مادة في المركز، أية مسؤولية عن أيّ ضرر مباشر أو غير مباشر مادي أو معنوي ينشأ من استعمال هذا الموقع، أو من عدم التمكن من استعماله، أو من أي خطأ أو حذف، أو عيب، يوجد فيه، أو من عدم صحة المعلومات التي يقدمها أو من أي تأخير أو انقطاع في بثّه.\r\n\r\nمعاودة النشر: لا يحقّ للباحث الذي قدّم بحثًا في أحد مؤتمرات المركز، أو نُشر له في إحدى دوريات المركز، أن يعيد نشره إلا بموجب إذنٍ خطّي مُسبق من إدارة المركز. وفي حال الموافقة، يشترط عليه أن يشير إلى أنّ هذا البحث قدم إلى مؤتمر المركز أو إلى الدورية المعنية مع تحديد العدد والتاريخ.\r\n\r\nأحكام عامة: تطبق على هذه الشروط قوانين دولة قطر ويكون لمحاكمها سلطة حصرية في نظر النزاعات الناشئة عنها.\r\n\r\n", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null, "اتفاقية الاستخدام للعمال" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "SuspendReasons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SuspendReasons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SuspendReasons",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SuspendReasons",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TermsAndCondetions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TermsAndCondetions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ForClient",
                table: "TermsAndCondetions");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم قبول طلبك. اضغط هنا للذهاب إلى تفاصيل الطلب.", "الطلب مقبول" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم تأكيد طلبك. اضغط هنا لاختيار احد العروض المقدمة من العمال.", "تم تأكيد الطلب" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم قبول العرض المقدم من العامل. اضغط هنا لاستكمال الدفع.", "تم تعيين العامل" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم انتهاء طلبك. اضغط هنا لتقييم مدى رضاك عن آداء العامل.", "انتهاء الطلب" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم إلغاء طلبك. اذا كنت مازلت تحتاج الخدمة برجاء انشاء طلب جديد.", "الطلب ملغي" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Type" },
                values: new object[] { " تفاصيل الاشعار.", "اشعار جديد" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ForClient", "Title", "Type" },
                values: new object[] { false, "تم قبول عرضك على الطلب رقم <id># اضغط هنا للذهاب إلى تفاصيل الطلب.", "العرض مقبول" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم إضافة مبلغ <price> دينار إلى محفظتك لدفع رسوم الطلب رقم <id>#. الذهاب الى المحفظة", "تم تعديل الرصيد" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Title", "Type" },
                values: new object[] { "يتوافق الطلب رقم <id># مع خدماتك. اضغط هنا للاطلاع على التفاصيل ", "طلب خدمة جديد" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم انتهاء الطلب رقم <id># اضغط هنا لتقييم تجربتك مع العميل.", "انتهاء الطلب" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تم إلغاء الطلب رقم <id># وإلغاء الموعد المسجل لتقديم الخدمة.", "الطلب ملغي" });

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Title", "Type" },
                values: new object[] { "تفاصيل الاشعار.", "اشعار جديد" });
        }
    }
}
