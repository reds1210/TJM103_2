const li = {
    props: ['item'],//外部傳入
    $emit: ['check-item'],
    template: `<li>
                <input type=checkbox @click="$emit('check-item',item.title)">
                    <span>{{item.title}}</span>
                    <slot></slot>
               </li>`
}
